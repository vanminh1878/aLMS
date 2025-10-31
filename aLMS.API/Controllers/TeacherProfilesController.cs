// aLMS.API.Controllers/TeacherProfilesController.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.TeacherProfileServices.Commands.CreateTeacherProfile;
using aLMS.Application.TeacherProfileServices.Commands.DeleteTeacherProfile;
using aLMS.Application.TeacherProfileServices.Commands.UpdateTeacherProfile;
using aLMS.Application.TeacherProfileServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/teacher-profiles")]
public class TeacherProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeacherProfilesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{userId}")]
    public async Task<ActionResult<TeacherProfileDto>> GetByUserId(Guid userId)
    {
        var profile = await _mediator.Send(new GetTeacherProfileQuery { UserId = userId });
        return profile == null ? NotFound() : Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult<CreateTeacherProfileResult>> Create([FromBody] CreateTeacherProfileDto dto)
    {
        var result = await _mediator.Send(new CreateTeacherProfileCommand { Dto = dto });
        return result.Success ? CreatedAtAction(nameof(GetByUserId), new { userId = result.UserId }, result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateTeacherProfileResult>> Update([FromBody] UpdateTeacherProfileDto dto)
    {
        var result = await _mediator.Send(new UpdateTeacherProfileCommand { Dto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult<DeleteTeacherProfileResult>> Delete(Guid userId)
    {
        var result = await _mediator.Send(new DeleteTeacherProfileCommand { UserId = userId });
        return result.Success ? Ok(result) : BadRequest(result);
    }
}