// aLMS.API.Controllers/LessonsController.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.LessonServices.Commands;
using aLMS.Application.LessonServices.Commands.CreateLesson;
using aLMS.Application.LessonServices.Commands.DeleteLesson;
using aLMS.Application.LessonServices.Commands.UpdateLesson;
using aLMS.Application.LessonServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LessonsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetAll()
        {
            var lessons = await _mediator.Send(new GetAllLessonsQuery());
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetById(Guid id)
        {
            var lesson = await _mediator.Send(new GetLessonByIdQuery { Id = id });
            return lesson == null ? NotFound() : Ok(lesson);
        }

        [HttpGet("by-topic/{topicId}")]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetByTopicId(Guid topicId)
        {
            var lessons = await _mediator.Send(new GetLessonsByTopicIdQuery { TopicId = topicId });
            return Ok(lessons);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateLessonDto dto)
        {
            var id = await _mediator.Send(new CreateLessonCommand { LessonDto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateLessonDto dto)
        {
            var result = await _mediator.Send(new UpdateLessonCommand { LessonDto = dto });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteLessonCommand { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}