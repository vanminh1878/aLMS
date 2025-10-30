using aLMS.Application.Common.Dtos;
using aLMS.Application.GradeServices.Commands.CreateGrade;
using aLMS.Application.GradeServices.Commands.DeleteGrade;
using aLMS.Application.GradeServices.Commands.UpdateGrade;
using aLMS.Application.GradeServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GradesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetAll()
        {
            var grades = await _mediator.Send(new GetAllGradesQuery());
            return Ok(grades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GradeDto>> GetById(Guid id)
        {
            var grade = await _mediator.Send(new GetGradeByIdQuery { Id = id });
            return grade == null ? NotFound() : Ok(grade);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateGradeDto dto)
        {
            var id = await _mediator.Send(new CreateGradeCommand { GradeDto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateGradeDto dto)
        {
            var result = await _mediator.Send(new UpdateGradeCommand { GradeDto = dto });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteGradeCommand { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}