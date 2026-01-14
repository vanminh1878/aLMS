using aLMS.Application.Common.Dtos;
using aLMS.Application.StudentEvaluationServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aLMS.Application.StudentEvaluationServices.Commands;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/student-evaluations")]
    public class StudentEvaluationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentEvaluationsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateStudentEvaluationDto dto)
        {
            var id = await _mediator.Send(new CreateStudentEvaluationCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentEvaluationDto dto)
        {
            dto.Id = id;
            var success = await _mediator.Send(new UpdateStudentEvaluationCommand { Dto = dto });
            return success ? Ok() : NotFound();
        }

        [HttpGet("by-student/{studentId}")]
        public async Task<ActionResult<IEnumerable<StudentEvaluationDto>>> GetByStudentId(
            Guid studentId,
            [FromQuery] string? semester = null,
            [FromQuery] string? schoolYear = null)
        {
            var query = new GetStudentEvaluationsByStudentQuery
            {
                StudentId = studentId,
                Semester = semester,
                SchoolYear = schoolYear
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("by-class/{classId}")]
        public async Task<ActionResult<IEnumerable<StudentEvaluationDto>>> GetByClassId(
            Guid classId,
            [FromQuery] string? semester = null,
            [FromQuery] string? schoolYear = null)
        {
            var query = new GetStudentEvaluationsByClassQuery
            {
                ClassId = classId,
                Semester = semester,
                SchoolYear = schoolYear
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{evaluationId}/subject-comments")]
        public async Task<ActionResult<Guid>> AddSubjectComment(Guid evaluationId, [FromBody] CreateStudentSubjectCommentDto dto)
        {
            dto.StudentEvaluationId = evaluationId;
            var id = await _mediator.Send(new AddSubjectCommentCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPost("{evaluationId}/quality-evaluations")]
        public async Task<ActionResult<Guid>> AddQualityEvaluation(Guid evaluationId, [FromBody] CreateStudentQualityEvaluationDto dto)
        {
            dto.StudentEvaluationId = evaluationId;
            var id = await _mediator.Send(new AddQualityEvaluationCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentEvaluationDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetStudentEvaluationByIdQuery { Id = id });
            return result == null ? NotFound() : Ok(result);
        }
    }
}