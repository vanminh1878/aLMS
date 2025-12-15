using aLMS.Application.BehaviourServices.Commands;
using aLMS.Application.BehaviourServices.Queries;
using aLMS.Application.Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/students/{studentId}/behaviours")]
public class BehavioursController : ControllerBase
{
    private readonly IMediator _mediator;

    public BehavioursController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BehaviourDto>>> GetByStudent(Guid studentId)
    {
        var behaviours = await _mediator.Send(new GetBehavioursByStudentQuery { StudentId = studentId });
        return Ok(behaviours);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BehaviourDto>> GetById(Guid studentId, Guid id)
    {
        var behaviour = await _mediator.Send(new GetBehaviourByIdQuery { Id = id });
        return behaviour == null ? NotFound() : Ok(behaviour);
    }
    
    [HttpPost]
    public async Task<ActionResult<BehaviourDto>> Create(Guid studentId, CreateBehaviourCommand command)
    {
        command.StudentId = studentId;

        var createdBehaviour = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { studentId = studentId, id = createdBehaviour.Id },
            createdBehaviour);
    }
    [HttpGet("~/api/behaviours/by-class")]
    public async Task<ActionResult<List<StudentBehaviourSummaryDto>>> GetBehavioursByClass([FromQuery] Guid classId)
    {
        if (classId == Guid.Empty)
            return BadRequest("ClassId là bắt buộc");

        var result = await _mediator.Send(new GetBehavioursByClassQuery { ClassId = classId });

        return result.Count == 0
            ? NotFound("Không tìm thấy hành vi nào cho lớp này")
            : Ok(result);
    }

}