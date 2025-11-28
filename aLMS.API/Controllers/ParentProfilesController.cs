using aLMS.Application.Common.Dtos;
using aLMS.Application.ParentProfileServices.Commands.CreateParentProfile;
using aLMS.Application.ParentProfileServices.Commands.DeleteParentProfile;
using aLMS.Application.ParentProfileServices.Commands.UpdateParentProfile;
using aLMS.Application.ParentProfileServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/parent-profiles")]
public class ParentProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParentProfilesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("parent/{parentId}")]
    public async Task<ActionResult<ParentProfileDto>> GetByParent(Guid parentId)
    {
        var profile = await _mediator.Send(new GetParentProfileQuery { ParentId = parentId });
        return profile == null ? NotFound() : Ok(profile);
    }

    [HttpGet("student/{studentId}/parents")]
    public async Task<ActionResult<IEnumerable<ParentProfileDto>>> GetParentsByStudent(Guid studentId)
    {
        var profiles = await _mediator.Send(new GetParentsByStudentQuery { StudentId = studentId });
        return Ok(profiles);
    }

    [HttpGet("parent/{parentId}/children")]
    public async Task<ActionResult<IEnumerable<ParentProfileDto>>> GetChildrenByParent(Guid parentId)
    {
        var profiles = await _mediator.Send(new GetChildrenByParentQuery { ParentId = parentId });
        return Ok(profiles);
    }

    [HttpPost]
    public async Task<ActionResult<CreateParentProfileResult>> Create([FromBody] CreateParentProfileDto dto)
    {
        var result = await _mediator.Send(new CreateParentProfileCommand { Dto = dto });
        return result.Success ? CreatedAtAction(nameof(GetByParent), new { parentId = result.ParentId }, result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateParentProfileResult>> Update([FromBody] UpdateParentProfileDto dto)
    {
        var result = await _mediator.Send(new UpdateParentProfileCommand { Dto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete]
    public async Task<ActionResult<DeleteParentProfileResult>> Delete([FromBody] DeleteParentProfileDto dto)
    {
        var result = await _mediator.Send(new DeleteParentProfileCommand { ParentId = dto.ParentId, StudentId = dto.StudentId });
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
public class DeleteParentProfileDto
{
    public Guid ParentId { get; set; }
    public Guid StudentId { get; set; }
}