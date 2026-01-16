using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.DTOs;
using aLMS.Application.SubjectServices.Commands.CreateSubject;
using aLMS.Application.SubjectServices.Commands.DeleteSubject;
using aLMS.Application.SubjectServices.Commands.UpdateSubject;
using aLMS.Application.SubjectServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubjectsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAll()
        {
            var subjects = await _mediator.Send(new GetAllSubjectsQuery());
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetById(Guid id)
        {
            var subject = await _mediator.Send(new GetSubjectByIdQuery { Id = id });
            return subject == null ? NotFound() : Ok(subject);
        }

        [HttpGet("by-class/{classId}")]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetByClassId(Guid classId)
        {
            var subjects = await _mediator.Send(new GetSubjectsByClassIdQuery { ClassId = classId });
            return Ok(subjects);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateSubjectDto dto)
        {
            var id = await _mediator.Send(new CreateSubjectCommand { SubjectDto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSubjectDto dto)
        {
            var result = await _mediator.Send(new UpdateSubjectCommand { SubjectDto = dto });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteSubjectCommand { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("assigned-to-teacher/{teacherId}")]
        public async Task<ActionResult<List<AssignedSubjectDto>>> GetAssignedSubjects(
            Guid teacherId,
            [FromQuery] string? schoolYear = null)
        {
            var query = new GetAssignedSubjectsByTeacherQuery
            {
                TeacherId = teacherId,
                SchoolYear = schoolYear
            };

            var subjects = await _mediator.Send(query);

            return Ok(subjects);
        }
    }
}