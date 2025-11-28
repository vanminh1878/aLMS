using aLMS.Application.Common.Dtos;
using aLMS.Application.RolePermissionServices.Commands.AssignPermission;
using aLMS.Application.RolePermissionServices.Commands.RemovePermission;
using aLMS.Application.RolePermissionServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/roles/{roleId}/permissions")]
public class RolePermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolePermissionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RolePermissionDto>>> GetByRole(Guid roleId)
    {
        var result = await _mediator.Send(new GetPermissionsByRoleQuery { RoleId = roleId });
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AssignPermissionResult>> Assign(Guid roleId, [FromBody] AssignPermissionDto dto)
    {
        dto.RoleId = roleId;
        var result = await _mediator.Send(new AssignPermissionCommand { Dto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{permissionId}")]
    public async Task<ActionResult<RemovePermissionResult>> Remove(Guid roleId, Guid permissionId)
    {
        var result = await _mediator.Send(new RemovePermissionCommand { RoleId = roleId, PermissionId = permissionId });
        return result.Success ? Ok(result) : BadRequest(result);
    }
}