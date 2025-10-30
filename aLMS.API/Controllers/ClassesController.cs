using aLMS.Application.Common.Dtos;
using aLMS.Application.ClassServices.Commands.CreateClass;
using aLMS.Application.ClassServices.Commands.DeleteClass;
using aLMS.Application.ClassServices.Commands.UpdateClass;
using aLMS.Application.ClassServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetAllClasses()
        {
            var classes = await _mediator.Send(new GetAllClassesQuery());
            return Ok(classes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassDto>> GetClassById(Guid id)
        {
            var classEntity = await _mediator.Send(new GetClassByIdQuery { Id = id });
            return classEntity == null ? NotFound() : Ok(classEntity);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateClass([FromBody] CreateClassDto classDto)
        {
            var classId = await _mediator.Send(new CreateClassCommand { ClassDto = classDto });
            return CreatedAtAction(nameof(GetClassById), new { id = classId }, classId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClass([FromBody] UpdateClassDto classDto)
        {
            var result = await _mediator.Send(new UpdateClassCommand { ClassDto = classDto });
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(Guid id)
        {
            var result = await _mediator.Send(new DeleteClassCommand { Id = id });

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("by-grade/{gradeId}")]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetByGradeId(Guid gradeId)
        {
            var classes = await _mediator.Send(new GetClassesByGradeIdQuery { GradeId = gradeId });
            return Ok(classes);
        }
    }
}