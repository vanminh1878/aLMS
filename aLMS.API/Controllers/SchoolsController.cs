using aLMS.Application.Common.Dtos;
using aLMS.Application.SchoolServices.Commands.CreateSchool;
using aLMS.Application.SchoolServices.Commands.DeleteSchool;
using aLMS.Application.SchoolServices.Commands.UpdateSchool;
using aLMS.Application.SchoolServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MassTransit.ValidationResultExtensions;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SchoolsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolDto>>> GetAllSchools()
        {
            var schools = await _mediator.Send(new GetAllSchoolsQuery());
            return Ok(schools);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolDto>> GetSchoolById(Guid id)
        {
            var school = await _mediator.Send(new GetSchoolByIdQuery { Id = id });
            return school == null ? NotFound() : Ok(school);
        }


        [HttpPost]
        public async Task<ActionResult<Guid>> CreateSchool([FromBody] CreateSchoolDto schoolDto)
        {
            var schoolId = await _mediator.Send(new CreateSchoolCommand { SchoolDto = schoolDto });
            return CreatedAtAction(nameof(GetSchoolById), new { id = schoolId }, schoolId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSchool([FromBody] UpdateSchoolDto schoolDto)
        {
            var result = await _mediator.Send(new UpdateSchoolCommand { SchoolDto = schoolDto });
            if (!result.Success)
                return BadRequest(result); 

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchool(Guid id)
        {
            var result = await _mediator.Send(new DeleteSchoolCommand { Id = id });

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


    }
}