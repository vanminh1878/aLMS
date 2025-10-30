// aLMS.API.Controllers/ExercisesController.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.ExerciseServices.Commands.CreateExercise;
using aLMS.Application.ExerciseServices.Commands.DeleteExercise;
using aLMS.Application.ExerciseServices.Commands.UpdateExercise;
using aLMS.Application.ExerciseServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExercisesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAll()
        {
            var exercises = await _mediator.Send(new GetAllExercisesQuery());
            return Ok(exercises);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDto>> GetById(Guid id)
        {
            var exercise = await _mediator.Send(new GetExerciseByIdQuery { Id = id });
            return exercise == null ? NotFound() : Ok(exercise);
        }

        [HttpGet("by-lesson/{lessonId}")]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetByLessonId(Guid lessonId)
        {
            var exercises = await _mediator.Send(new GetExercisesByLessonIdQuery { LessonId = lessonId });
            return Ok(exercises);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateExerciseDto dto)
        {
            var id = await _mediator.Send(new CreateExerciseCommand { ExerciseDto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateExerciseDto dto)
        {
            var result = await _mediator.Send(new UpdateExerciseCommand { ExerciseDto = dto });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteExerciseCommand { Id = id });
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}