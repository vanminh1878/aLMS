using aLMS.Application.Common.Dtos;
using aLMS.Application.TimetableServices.Commands;
using aLMS.Application.TimetableServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/timetables")]
    public class TimetablesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TimetablesController(IMediator mediator) => _mediator = mediator;

        // POST /api/timetables/generate
        [HttpPost("generate")]
        public async Task<ActionResult<GenerateTimetableResult>> Generate([FromBody] GenerateTimetableDto dto)
        {
            var result = await _mediator.Send(new GenerateTimetableCommand { Dto = dto });
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        // GET /api/timetables/by-class/{classId}?schoolYear=...
        [HttpGet("by-class/{classId}")]
        public async Task<ActionResult<IEnumerable<TimetableDto>>> GetByClass(
            Guid classId,
            [FromQuery] string? schoolYear = null)
        {
            var query = new GetTimetableByClassQuery
            {
                ClassId = classId,
                SchoolYear = schoolYear
            };

            var timetables = await _mediator.Send(query);
            return Ok(timetables);
        }

        // GET /api/timetables/by-teacher/{teacherId}?schoolYear=...
        [HttpGet("by-teacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<TimetableDto>>> GetByTeacher(
            Guid teacherId,
            [FromQuery] string? schoolYear = null)
        {
            var query = new GetTimetableByTeacherQuery
            {
                TeacherId = teacherId,
                SchoolYear = schoolYear
            };

            var timetables = await _mediator.Send(query);
            return Ok(timetables);
        }

        // GET /api/timetables/by-student/{studentId}?schoolYear=...
        [HttpGet("by-student/{studentId}")]
        public async Task<ActionResult<IEnumerable<TimetableDto>>> GetByStudent(
            Guid studentId,
            [FromQuery] string? schoolYear = null)
        {
            var query = new GetTimetableByStudentQuery
            {
                StudentId = studentId,
                SchoolYear = schoolYear
            };

            var timetables = await _mediator.Send(query);
            return Ok(timetables);
        }
        // POST /api/timetables (thêm tay 1 tiết)
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTimetableDto dto)
        {
            var id = await _mediator.Send(new CreateTimetableCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id); // GetById nếu có sau
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TimetableDto>> GetById(Guid id)
        {
            var timetable = await _mediator.Send(new GetTimetableByIdQuery { Id = id });
            return timetable == null ? NotFound() : Ok(timetable);
        }

        // PUT /api/timetables/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTimetableDto dto)
        {
            dto.Id = id;
            var success = await _mediator.Send(new UpdateTimetableCommand { Dto = dto });
            return success ? Ok() : NotFound();
        }

        // DELETE /api/timetables/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(new DeleteTimetableCommand { Id = id });
            return success ? NoContent() : NotFound();
        }
    }
}