using aLMS.Application.Common.Dtos;
using aLMS.Application.DepartmentServices.Commands.CreateDepartment;
using aLMS.Application.DepartmentServices.Commands.DeleteDepartment;
using aLMS.Application.DepartmentServices.Commands.UpdateDepartment;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

[ApiController]
[Route("api/schools/{schoolId}/departments")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetBySchool(Guid schoolId)
    {
        var departments = await _mediator.Send(new GetDepartmentsBySchoolQuery { SchoolId = schoolId });
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> GetById(Guid schoolId, Guid id)
    {
        var department = await _mediator.Send(new GetDepartmentByIdQuery { Id = id });
        return department == null ? NotFound() : Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<CreateDepartmentResult>> Create( [FromBody] CreateDepartmentDto dto)
    {
        var result = await _mediator.Send(new CreateDepartmentCommand { Dto = dto });
        return Ok(new { success = true, data = result });
    }

    [HttpPut]
    public async Task<ActionResult<UpdateDepartmentResult>> Update([FromBody] UpdateDepartmentDto dto)
    {
        var result = await _mediator.Send(new UpdateDepartmentCommand { Dto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteDepartmentResult>> Delete(Guid schoolId, Guid id)
    {
        var result = await _mediator.Send(new DeleteDepartmentCommand { Id = id });
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpPost("{departmentId}/add-teachers")]
    public async Task<ActionResult<AddTeachersToDepartmentResult>> AddTeachersToDepartment(
        Guid departmentId,
        [FromBody] List<AddTeacherToDepartmentDto> dtos)
    {
        var result = await _mediator.Send(new AddTeachersToDepartmentCommand
        {
            DepartmentId = departmentId,
            Teachers = dtos
        });

        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }
    [HttpPost("{departmentId}/add-teachers/excel")]
    public async Task<ActionResult<AddTeachersToDepartmentResult>> AddTeachersFromExcel(
       Guid departmentId,
       [FromForm] AddTeachersFromExcelRequest request)
    {
        var file = request.File;

        if (file == null || file.Length == 0)
            return BadRequest("Vui lòng upload file Excel");

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Chỉ hỗ trợ định dạng .xlsx");

        if (request.SchoolId == Guid.Empty)
            return BadRequest("SchoolId không hợp lệ");

        var dtos = new List<AddTeacherToDepartmentDto>();
        var errors = new List<string>();

        try
        {
            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1); // sheet đầu tiên

            // Bắt đầu từ hàng 5
            int row = 5;

            while (!worksheet.Cell(row, 2).IsEmpty()) // cột B - Họ tên GV
            {
                try
                {
                    var dto = new AddTeacherToDepartmentDto
                    {
                        FullName = worksheet.Cell(row, 2).GetString().Trim(),
                        DateOfBirth = ParseDate(worksheet.Cell(row, 3).GetString()),
                        Gender = worksheet.Cell(row, 4).GetString().Trim(),
                        HireDate = ParseDate(worksheet.Cell(row, 5).GetString()),
                        Phone = CleanPhone(worksheet.Cell(row, 6).GetString()),
                        Email = worksheet.Cell(row, 7).GetString().Trim(),
                        Address = worksheet.Cell(row, 8).GetString().Trim(),
                        Specialization = worksheet.Cell(row, 9).GetString().Trim(),

                        // Gán SchoolId từ request
                        SchoolId = request.SchoolId
                    };

                    // Validate cơ bản
                    if (string.IsNullOrWhiteSpace(dto.FullName))
                        throw new Exception("Thiếu họ tên giáo viên");
                    if (string.IsNullOrWhiteSpace(dto.Phone))
                        throw new Exception("Thiếu số điện thoại");

                    dtos.Add(dto);
                }
                catch (Exception ex)
                {
                    errors.Add($"Dòng {row}: {ex.Message}");
                }

                row++;
            }

            if (!dtos.Any())
                return BadRequest("Không tìm thấy dữ liệu giáo viên nào trong file Excel (từ hàng 5 trở đi)");

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

        // Gửi command giống API JSON
        var command = new AddTeachersToDepartmentCommand
        {
            DepartmentId = departmentId,
            Teachers = dtos
        };

        var result = await _mediator.Send(command);

        // Thêm thông báo nếu có lỗi Excel nhưng vẫn xử lý được một phần
        if (errors.Any())
        {
            result.Message += $"\n(Lưu ý: Excel có {errors.Count} dòng lỗi nhưng vẫn xử lý {dtos.Count} giáo viên hợp lệ)";
        }

        return result.Success ? Ok(result) : BadRequest(result);
    }

    // Hàm hỗ trợ (có thể đặt ở class riêng hoặc static helper)
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
    public class AddTeachersFromExcelRequest
    {
        public IFormFile File { get; set; } = null!;
        public Guid SchoolId { get; set; }
    }
}