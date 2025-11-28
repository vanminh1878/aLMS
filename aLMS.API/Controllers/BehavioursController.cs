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

}