using aLMS.Application.Common.Dtos;
using aLMS.Application.ClassSubjectServices.Commands.CreateClassSubject;
using aLMS.Application.ClassSubjectServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/class-subjects")]
    public class ClassSubjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassSubjectsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateClassSubjectDto dto)
        {
            var id = await _mediator.Send(new CreateClassSubjectCommand { ClassSubjectDto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassSubjectDto>> GetById(Guid id)
        {
            var classSubject = await _mediator.Send(new GetClassSubjectByIdQuery { Id = id });
            return classSubject == null ? NotFound() : Ok(classSubject);
        }
        [HttpGet("by-class/{classId}")]
        public async Task<ActionResult<IEnumerable<ClassSubjectDto>>> GetByClassId(Guid classId)
        {
            var classSubjects = await _mediator.Send(new GetClassSubjectsByClassIdQuery { ClassId = classId });
            return Ok(classSubjects);
        }

    }
}