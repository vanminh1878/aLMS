using aLMS.Application.Common.Dtos;
using aLMS.Application.NotificationServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aLMS.Application.NotificationServices.Commands;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateNotificationDto dto)
        {
            var id = await _mediator.Send(new CreateNotificationCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNotificationDto dto)
        {
            dto.Id = id;
            var success = await _mediator.Send(new UpdateNotificationCommand { Dto = dto });
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(new DeleteNotificationCommand { Id = id });
            return success ? NoContent() : NotFound();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetById(Guid id)
        {
            var notification = await _mediator.Send(new GetNotificationByIdQuery { Id = id });
            return notification == null ? NotFound() : Ok(notification);
        }

        [HttpGet("by-class/{classId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetByClassId(Guid classId)
        {
            var notifications = await _mediator.Send(new GetNotificationsByClassQuery { ClassId = classId });
            return Ok(notifications);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetByUserId(Guid userId)
        {
            var notifications = await _mediator.Send(new GetNotificationsByUserQuery { UserId = userId });
            return Ok(notifications);
        }

        [HttpPost("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var success = await _mediator.Send(new MarkNotificationAsReadCommand { Id = id });
            return success ? Ok() : NotFound();
        }
    }
}