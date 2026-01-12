using aLMS.Application.Common.Dtos;
using aLMS.Application.ClassSubjectTeacherServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aLMS.Application.ClassSubjectTeacherServices.Commands.AddClassSubjectTeacher;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/class-subjects")]
    public class ClassSubjectTeachersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassSubjectTeachersController(IMediator mediator) => _mediator = mediator;

        // POST /api/class-subjects/{classSubjectId}/teachers
        [HttpPost("{classSubjectId}/teachers")]
        public async Task<ActionResult<Guid>> AddTeacher(Guid classSubjectId, [FromBody] AddClassSubjectTeacherDto dto)
        {
            var command = new AddClassSubjectTeacherCommand
            {
                ClassSubjectId = classSubjectId,
                TeacherId = dto.TeacherId,
                SchoolYear = dto.SchoolYear
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetTeachersByClassSubject), new { classSubjectId }, result.Id);
        }

        // GET /api/class-subjects/{classSubjectId}/teachers
        [HttpGet("{classSubjectId}/teachers")]
        public async Task<ActionResult<IEnumerable<ClassSubjectTeacherDto>>> GetTeachersByClassSubject(Guid classSubjectId)
        {
            var teachers = await _mediator.Send(new GetTeachersByClassSubjectQuery { ClassSubjectId = classSubjectId });
            return Ok(teachers);
        }

        // GET /api/class-subjects/by-teacher/{teacherId}
        [HttpGet("by-teacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<ClassSubjectDto>>> GetClassSubjectsByTeacher(Guid teacherId)
        {
            var classSubjects = await _mediator.Send(new GetClassSubjectsByTeacherQuery { TeacherId = teacherId });
            return Ok(classSubjects);
        }
    }
}