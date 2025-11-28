using aLMS.Application.AuthServices.Commands.Login;
using aLMS.Application.AuthServices.Commands.RefreshToken;
using aLMS.Application.Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _mediator.Send(new LoginCommand { Dto = dto });
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenDto dto)
    {
        var result = await _mediator.Send(new RefreshTokenCommand { Dto = dto });
        return result.Success ? Ok(result) : Unauthorized(result);
    }
}