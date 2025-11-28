using aLMS.Application.Common.Dtos;
using aLMS.Application.PermissionServices.Commands.CreatePermission;
using aLMS.Application.PermissionServices.Commands.DeletePermission;
using aLMS.Application.PermissionServices.Commands.UpdatePermission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetAll()
    {
        var permissions = await _mediator.Send(new GetAllPermissionsQuery());
        return Ok(permissions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PermissionDto>> GetById(Guid id)
    {
        var permission = await _mediator.Send(new GetPermissionByIdQuery { Id = id });
        return permission == null ? NotFound() : Ok(permission);
    }

    [HttpPost]
    public async Task<ActionResult<CreatePermissionResult>> Create([FromBody] CreatePermissionDto dto)
    {
        var result = await _mediator.Send(new CreatePermissionCommand { PermissionDto = dto });
        return result.Success ? CreatedAtAction(nameof(GetById), new { id = result.PermissionId }, result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatePermissionResult>> Update([FromBody] UpdatePermissionDto dto)
    {
        var result = await _mediator.Send(new UpdatePermissionCommand { PermissionDto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletePermissionResult>> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeletePermissionCommand { Id = id });
        return result.Success ? Ok(result) : BadRequest(result);
    }
}