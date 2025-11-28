using aLMS.Application.ClassServices.Commands.CreateClass;
using aLMS.Application.ClassServices.Commands.DeleteClass;
using aLMS.Application.ClassServices.Commands.UpdateClass;
using aLMS.Application.ClassServices.Queries;
using aLMS.Application.Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/classes")]
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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClassDto>> GetClassById(Guid id)
    {
        var result = await _mediator.Send(new GetClassByIdQuery { Id = id });
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateClass([FromBody] CreateClassDto dto)
    {
        var classId = await _mediator.Send(new CreateClassCommand { ClassDto = dto });
        return CreatedAtAction(nameof(GetClassById), new { id = classId }, classId);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateClass([FromBody] UpdateClassDto dto)
    {
        var result = await _mediator.Send(new UpdateClassCommand { ClassDto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeactivateClass(Guid id)
    {
        var result = await _mediator.Send(new DeleteClassCommand { Id = id });
        return result.Success ? NoContent() : BadRequest(result);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses(
        [FromQuery] string? grade = null,
        [FromQuery] string? schoolYear = null)
    {
        var classes = await _mediator.Send(new GetClassesFilteredQuery
        {
            Grade = grade,
            SchoolYear = schoolYear
        });
        return Ok(classes);
    }
}