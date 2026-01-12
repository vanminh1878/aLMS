using aLMS.Application.ClassServices.Commands.AddStudentsToClass;
using aLMS.Application.ClassServices.Commands.CreateClass;
using aLMS.Application.ClassServices.Commands.DeleteClass;
using aLMS.Application.ClassServices.Commands.UpdateClass;
using aLMS.Application.ClassServices.Queries;
using aLMS.Application.ClassSubjectServices.Commands.AddSubjectToClass;
using aLMS.Application.ClassSubjectServices.Queries;
using aLMS.Application.Common.Dtos;
using aLMS.Application.SubjectServices.Queries;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static MassTransit.ValidationResultExtensions;

[ApiController]
[Route("api/classes")]
public class ClassesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClassesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{classId}/subjects")]
    public async Task<ActionResult<Guid>> AddSubjectToClass(Guid classId, [FromBody] AddSubjectToClassDto dto)
    {
        var command = new AddSubjectToClassCommand
        {
            ClassId = classId,
            SubjectId = dto.SubjectId,
            SchoolYear = dto.SchoolYear
        };

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return CreatedAtAction(nameof(GetSubjectsByClassId), new { classId }, result.ClassSubjectId);
    }

    [HttpGet("{classId}/subjects")]
    public async Task<ActionResult<IEnumerable<ClassSubjectDto>>> GetSubjectsByClassId(Guid classId)
    {
        var subjects = await _mediator.Send(new GetSubjectsByClassQuery { ClassId = classId });
        return Ok(subjects);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassDto>>> GetAllClasses()
    {
        var classes = await _mediator.Send(new GetAllClassesQuery());
        return Ok(classes);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClassDto>> GetClassById(Guid id)
    {
        var result = await _mediator.Send(new GetClassByIdQuery { Id = id });
        return result == null ? NotFound() : Ok(result);
    }
    [HttpGet("by-homeroom-teacher/{homeroomTeacherId:guid}")]
    public async Task<ActionResult<ClassDto>> GetClassByHomeroomTeacherId(Guid homeroomTeacherId)
    {
        var result = await _mediator.Send(new GetClassByHomeroomTeacherIdQuery
        {
            HomeroomTeacherId = homeroomTeacherId
        });

        return Ok(result ?? new ClassDto());
    }
    [HttpGet("by-school/{schoolId:guid}")]
    public async Task<ActionResult<IEnumerable<ClassDto>>> GetClassesBySchoolId(Guid schoolId)
    {
        var result = await _mediator.Send(new GetClassesBySchoolIdQuery { SchoolId = schoolId });
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateClass([FromBody] CreateClassDto dto)
    {
        var result = await _mediator.Send(new CreateClassCommand { ClassDto = dto });
        if (!result.Success)
        {
            return Problem(
                title: result.Message,
                detail: result.Message,
                statusCode: StatusCodes.Status409Conflict
            );
        }
        return CreatedAtAction(nameof(GetClassById), new { id = result.ClassId}, result.ClassId);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateClass([FromBody] UpdateClassDto dto)
    {
        var result = await _mediator.Send(new UpdateClassCommand { ClassDto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeactivateClass(Guid id)
    {
        var result = await _mediator.Send(new DeleteClassCommand { Id = id });
        return result.Success ? NoContent() : BadRequest(result);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses(
        [FromQuery] string? grade = null,
        [FromQuery] string? schoolYear = null)
    {
        var classes = await _mediator.Send(new GetClassesFilteredQuery
        {
            Grade = grade,
            SchoolYear = schoolYear
        });
        return Ok(classes);
    }
    [HttpPost("{classId}/add-students")]
    public async Task<ActionResult<AddStudentsToClassResult>> AddStudentsToClass(
    Guid classId,
    [FromBody] List<AddStudentToClassDto> dtos)
    {
        var result = await _mediator.Send(new AddStudentsToClassCommand
        {
            ClassId = classId,
            Students = dtos
        });

        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }
    // API mới - Excel
    [HttpPost("{classId}/add-students/excel")]
    public async Task<ActionResult<AddStudentsToClassResult>> AddStudentsFromExcel(
        Guid classId,
         IFormFile file,
         [FromForm] AddStudentsFromExcelRequest request) // SchoolId truyền từ form-data
    {
        if (file == null || file.Length == 0)
            return BadRequest("Vui lòng upload file Excel");

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Chỉ hỗ trợ định dạng .xlsx");

        var dtos = new List<AddStudentToClassDto>();
        var errors = new List<string>();

        try
        {
            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1); // sheet đầu tiên

            // Bắt đầu từ hàng 5
            int row = 5;

            while (!worksheet.Cell(row, 2).IsEmpty()) // cột B - Họ tên HS
            {
                try
                {
                    var dto = new AddStudentToClassDto
                    {
                        StudentName = worksheet.Cell(row, 2).GetString().Trim(),
                        StudentDateOfBirth = ParseDate(worksheet.Cell(row, 3).GetString()),
                        StudentEnrollDate = ParseDate(worksheet.Cell(row, 4).GetString()),
                        Address = worksheet.Cell(row, 5).GetString().Trim(),

                        ParentName = worksheet.Cell(row, 6).GetString().Trim(),
                        ParentDateOfBirth = ParseDate(worksheet.Cell(row, 7).GetString()),
                        ParentGender = worksheet.Cell(row, 8).GetString().Trim(),
                        ParentPhone = CleanPhone(worksheet.Cell(row, 9).GetString()),
                        ParentEmail = worksheet.Cell(row, 10).GetString().Trim(),

                        // Gán SchoolId từ request
                        SchoolId = request.SchoolId
                    };

                    // Validate cơ bản
                    if (string.IsNullOrWhiteSpace(dto.StudentName))
                        throw new Exception("Thiếu họ tên học sinh");
                    if (string.IsNullOrWhiteSpace(dto.ParentName))
                        throw new Exception("Thiếu họ tên phụ huynh");
                    if (string.IsNullOrWhiteSpace(dto.ParentPhone))
                        throw new Exception("Thiếu số điện thoại phụ huynh");

                    dtos.Add(dto);
                }
                catch (Exception ex)
                {
                    errors.Add($"Dòng {row}: {ex.Message}");
                }

                row++;
            }

            if (!dtos.Any())
                return BadRequest("Không tìm thấy dữ liệu học sinh nào trong file Excel (từ hàng 5 trở đi)");

            if (errors.Any())
            {
                return BadRequest(new
                {
                    Message = "File Excel có lỗi ở một số dòng",
                    Errors = errors,
                    TotalRowsRead = row - 5,
                    ValidRows = dtos.Count
                });
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Không thể đọc file Excel: {ex.Message}");
        }

        // Gửi command giống API cũ
        var command = new AddStudentsToClassCommand
        {
            ClassId = classId,
            Students = dtos
        };

        var result = await _mediator.Send(command);

        // Có thể thêm thông tin từ Excel vào result nếu muốn
        if (errors.Any())
        {
            result.Message += $"\n(Lưu ý: Excel có {errors.Count} dòng lỗi nhưng vẫn xử lý {dtos.Count} học sinh hợp lệ)";
        }

        return result.Success ? Ok(result) : BadRequest(result);
    }

    private static DateTime ParseDate(string value)
    {
        var formats = new[] { "dd/MM/yyyy", "dd-MM-yyyy", "d/M/yyyy", "d-M-yyyy" };
        if (DateTime.TryParseExact(value.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return date.Date;

        throw new Exception($"Định dạng ngày không hợp lệ: '{value}' (mong đợi dd/MM/yyyy)");
    }

    private static string CleanPhone(string phone)
    {
        return string.IsNullOrWhiteSpace(phone)
            ? ""
            : phone.Replace(" ", "").Replace("-", "").Replace(".", "").Replace("+84", "0").Trim();
    }
    [HttpGet("by-student/{studentId:guid}")]
    public async Task<ActionResult<IEnumerable<ClassDto>>> GetClassesByStudentId(Guid studentId)
    {
        var result = await _mediator.Send(new GetClassesByStudentIdQuery { StudentId = studentId });

        if (result == null || !result.Any())
        {
            return NotFound($"Không tìm thấy lớp nào cho học sinh với Id: {studentId}");
        }

        return Ok(result);
    }
    public class AddStudentsFromExcelRequest
    {
        public IFormFile File { get; set; } = null!;
        public Guid SchoolId { get; set; }
    }
}