using aLMS.Application.AccountServices.Commands.DeleteAccount;
using aLMS.Application.AccountServices.Commands.Register;
using aLMS.Application.AccountServices.Commands.UpdateAccount;
using aLMS.Application.AccountServices.Queries;
using aLMS.Application.AccountServices.Queries.Login;
using aLMS.Application.Common.Dtos;
using aLMS.Application.UserServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResult>> Register([FromBody] RegisterDto dto)
    {
        var result = await _mediator.Send(new RegisterCommand { Dto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetById(Guid id)
    {
        var account = await _mediator.Send(new GetAccountByIdQuery { Id = id });
        return account == null ? NotFound("Không tìm thấy tài khoản") : Ok(account);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginDto dto)
    {
        var result = await _mediator.Send(new LoginQuery { Dto = dto });
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateAccountResult>> Update([FromBody] UpdateAccountDto dto)
    {
        var result = await _mediator.Send(new UpdateAccountCommand { Dto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteAccountResult>> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteAccountCommand { Id = id });
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpGet("by-account/{accountId}")]
    public async Task<ActionResult<UserDto>> GetByAccountId(Guid accountId)
    {
        var user = await _mediator.Send(new GetUserByAccountIdQuery { AccountId = accountId });
        return user == null ? NotFound("User not found") : Ok(user);
    }
}