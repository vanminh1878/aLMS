// FinalTermRecordsController.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.FinalTermRecordServices.Commands;
using aLMS.Application.FinalTermRecordServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/final-term-records")]
    public class FinalTermRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FinalTermRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FinalTermRecordDto>> GetById(Guid id)
        {
            var record = await _mediator.Send(new GetFinalTermRecordByIdQuery { Id = id });
            if (record == null)
                return NotFound("Không tìm thấy bản ghi");

            return Ok(record);
        }

        [HttpGet("student/{studentProfileId}")]
        public async Task<ActionResult<List<FinalTermRecordDto>>> GetByStudent(Guid studentProfileId)
        {
            var records = await _mediator.Send(new GetFinalTermRecordsByStudentQuery
            {
                StudentProfileId = studentProfileId
            });

            return Ok(records);
        }

        [HttpGet("class/{classId}")]
        public async Task<ActionResult<List<FinalTermRecordDto>>> GetByClass(Guid classId)
        {
            var records = await _mediator.Send(new GetFinalTermRecordsByClassQuery
            {
                ClassId = classId
            });

            if (records.Count == 0)
                return NotFound("Không tìm thấy bản ghi nào cho lớp này");

            return Ok(records);
        }

        [HttpPost]
        public async Task<ActionResult<CreateFinalTermRecordResult>> Create([FromBody] CreateFinalTermRecordDto dto)
        {
            var result = await _mediator.Send(new CreateFinalTermRecordCommand { Dto = dto });

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = result.RecordId }, result);
        }

        [HttpPut]
        public async Task<ActionResult<UpdateFinalTermRecordResult>> Update([FromBody] UpdateFinalTermRecordDto dto)
        {
            var result = await _mediator.Send(new UpdateFinalTermRecordCommand { Dto = dto });

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}