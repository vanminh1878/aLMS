// aLMS.API.Controllers/StudentProfilesController.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.StudentProfileServices.Commands.CreateStudentProfile;
using aLMS.Application.StudentProfileServices.Commands.DeleteStudentProfile;
using aLMS.Application.StudentProfileServices.Commands.UpdateStudentProfile;
using aLMS.Application.StudentProfileServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/student-profiles")]
public class StudentProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentProfilesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{userId}")]
    public async Task<ActionResult<StudentProfileDto>> GetByUserId(Guid userId)
    {
        var profile = await _mediator.Send(new GetStudentProfileQuery { UserId = userId });
        return profile == null ? NotFound() : Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult<CreateStudentProfileResult>> Create([FromBody] CreateStudentProfileDto dto)
    {
        var result = await _mediator.Send(new CreateStudentProfileCommand { Dto = dto });
        return result.Success ? CreatedAtAction(nameof(GetByUserId), new { userId = result.UserId }, result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateStudentProfileResult>> Update([FromBody] UpdateStudentProfileDto dto)
    {
        var result = await _mediator.Send(new UpdateStudentProfileCommand { Dto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult<DeleteStudentProfileResult>> Delete(Guid userId)
    {
        var result = await _mediator.Send(new DeleteStudentProfileCommand { UserId = userId });
        return result.Success ? Ok(result) : BadRequest(result);
    }
}