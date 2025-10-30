using aLMS.Application.Common.Dtos;
using aLMS.Application.TopicServices.Commands.CreateTopic;
using aLMS.Application.TopicServices.Commands.DeleteTopic;
using aLMS.Application.TopicServices.Commands.UpdateTopic;
using aLMS.Application.TopicServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TopicsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TopicDto>>> GetAll()
        {
            var topics = await _mediator.Send(new GetAllTopicsQuery());
            return Ok(topics);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TopicDto>> GetById(Guid id)
        {
            var topic = await _mediator.Send(new GetTopicByIdQuery { Id = id });
            return topic == null ? NotFound() : Ok(topic);
        }

        [HttpGet("by-subject/{subjectId}")]
        public async Task<ActionResult<IEnumerable<TopicDto>>> GetBySubjectId(Guid subjectId)
        {
            var topics = await _mediator.Send(new GetTopicsBySubjectIdQuery { SubjectId = subjectId });
            return Ok(topics);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTopicDto dto)
        {
            var id = await _mediator.Send(new CreateTopicCommand { TopicDto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTopicDto dto)
        {
            var result = await _mediator.Send(new UpdateTopicCommand { TopicDto = dto });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteTopicCommand { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}