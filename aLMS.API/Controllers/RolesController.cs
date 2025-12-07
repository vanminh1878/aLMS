using aLMS.Application.Common.Dtos;
using aLMS.Application.RoleServices.Commands.CreateRole;
using aLMS.Application.RoleServices.Commands.DeleteRole;
using aLMS.Application.RoleServices.Commands.UpdateRole;
using aLMS.Application.RoleServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
    {
        var roles = await _mediator.Send(new GetAllRolesQuery());
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(Guid id)
    {
        var role = await _mediator.Send(new GetRoleByIdQuery { Id = id });
        return role == null ? NotFound() : Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<CreateRoleResult>> Create([FromBody] CreateRoleDto dto)
    {
        var result = await _mediator.Send(new CreateRoleCommand { RoleDto = dto });
        return result.Success ? CreatedAtAction(nameof(GetById), new { id = result.RoleId }, result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateRoleResult>> Update([FromBody] UpdateRoleDto dto)
    {
        var result = await _mediator.Send(new UpdateRoleCommand { RoleDto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteRoleResult>> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteRoleCommand { Id = id });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("by-name/{roleName}")]
    public async Task<ActionResult<Guid>> GetRoleIdByName(string roleName)
    {
        var roleId = await _mediator.Send(new GetRoleIdByNameQuery { RoleName = roleName });

        return roleId.HasValue
            ? Ok(roleId.Value)
            : NotFound($"Không tìm thấy role với tên: {roleName}");
    }
}