using aLMS.Application.Common.Dtos;
using aLMS.Application.UserServices.Commands.CreateUser;
using aLMS.Application.UserServices.Commands.DeleteUser;
using aLMS.Application.UserServices.Commands.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery { Id = id });
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResult>> Create([FromBody] CreateUserDto dto)
    {
        var result = await _mediator.Send(new CreateUserCommand { Dto = dto });
        return result.Success ? CreatedAtAction(nameof(GetById), new { id = result.UserId }, result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateUserResult>> Update([FromBody] UpdateUserDto dto)
    {
        var result = await _mediator.Send(new UpdateUserCommand { Dto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteUserResult>> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteUserCommand { Id = id });
        return result.Success ? Ok(result) : BadRequest(result);
    }
}