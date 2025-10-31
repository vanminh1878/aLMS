// aLMS.API.Controllers/DepartmentsController.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.DepartmentServices.Commands.CreateDepartment;
using aLMS.Application.DepartmentServices.Commands.DeleteDepartment;
using aLMS.Application.DepartmentServices.Commands.UpdateDepartment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/schools/{schoolId}/departments")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetBySchool(Guid schoolId)
    {
        var departments = await _mediator.Send(new GetDepartmentsBySchoolQuery { SchoolId = schoolId });
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> GetById(Guid schoolId, Guid id)
    {
        var department = await _mediator.Send(new GetDepartmentByIdQuery { Id = id });
        return department == null ? NotFound() : Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<CreateDepartmentResult>> Create(Guid schoolId, [FromBody] CreateDepartmentDto dto)
    {
        dto.SchoolId = schoolId;
        var result = await _mediator.Send(new CreateDepartmentCommand { Dto = dto });
        return result.Success ? CreatedAtAction(nameof(GetById), new { schoolId, id = result.DepartmentId }, result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateDepartmentResult>> Update([FromBody] UpdateDepartmentDto dto)
    {
        var result = await _mediator.Send(new UpdateDepartmentCommand { Dto = dto });
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteDepartmentResult>> Delete(Guid schoolId, Guid id)
    {
        var result = await _mediator.Send(new DeleteDepartmentCommand { Id = id });
        return result.Success ? Ok(result) : BadRequest(result);
    }
}