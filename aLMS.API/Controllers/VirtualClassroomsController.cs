using aLMS.Application.Common.Dtos;
using aLMS.Application.VirtualClassroomServices.Commands;
using aLMS.Application.VirtualClassroomServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/virtual-classrooms")]
    public class VirtualClassroomsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VirtualClassroomsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VirtualClassroomDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllVirtualClassroomsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VirtualClassroomDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetVirtualClassroomByIdQuery { Id = id });
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("by-class/{classId}")]
        public async Task<ActionResult<IEnumerable<VirtualClassroomDto>>> GetByClassId(Guid classId, [FromQuery] bool upcoming = false)
        {
            var query = new GetVirtualClassroomsByClassQuery
            {
                ClassId = classId,
                UpcomingOnly = upcoming
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("by-student/{studentId}")]
        public async Task<ActionResult<IEnumerable<VirtualClassroomDto>>> GetByStudentId(Guid studentId, [FromQuery] bool upcoming = false)
        {
            var query = new GetVirtualClassroomsByStudentQuery
            {
                StudentId = studentId,
                UpcomingOnly = upcoming
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateVirtualClassroomDto dto)
        {
            var id = await _mediator.Send(new CreateVirtualClassroomCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVirtualClassroomDto dto)
        {
            dto.Id = id;
            var success = await _mediator.Send(new UpdateVirtualClassroomCommand { Dto = dto });
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(new DeleteVirtualClassroomCommand { Id = id });
            return success ? NoContent() : NotFound();
        }
    }
}