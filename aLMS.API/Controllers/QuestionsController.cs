// aLMS.API.Controllers/QuestionsController.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.QuestionServices.Commands.CreateQuestion;
using aLMS.Application.QuestionServices.Commands.DeleteQuestion;
using aLMS.Application.QuestionServices.Commands.UpdateQuestion;
using aLMS.Application.QuestionServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/exercises/{exerciseId}/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuestionsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetByExercise(Guid exerciseId)
        {
            var questions = await _mediator.Send(new GetQuestionsByExerciseIdQuery { ExerciseId = exerciseId });
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetById(Guid exerciseId, Guid id)
        {
            var question = await _mediator.Send(new GetQuestionByIdQuery { Id = id });
            return question == null ? NotFound() : Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(Guid exerciseId, [FromBody] CreateQuestionDto dto)
        {
            dto.ExerciseId = exerciseId;
            var id = await _mediator.Send(new CreateQuestionCommand { QuestionDto = dto });
            return CreatedAtAction(nameof(GetById), new { exerciseId, id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateQuestionDto dto)
        {
            var result = await _mediator.Send(new UpdateQuestionCommand { QuestionDto = dto });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid exerciseId, Guid id)
        {
            var result = await _mediator.Send(new DeleteQuestionCommand { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}