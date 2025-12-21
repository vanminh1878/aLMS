using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.DTOs;
using aLMS.Application.StudentExerciseServices.Commands.StartExercise;
using aLMS.Application.StudentExerciseServices.Commands.SubmitExercise;
using aLMS.Application.StudentExerciseServices.Queries;
using aLMS.Domain.UserEntity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/exercises/{exerciseId}/student")]
public class StudentExercisesController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentExercisesController(IMediator mediator) => _mediator = mediator;

    [HttpPost("start")]
    public async Task<ActionResult<Guid>> Start(Guid exerciseId, Guid studentId)
    {
        var id = await _mediator.Send(new StartExerciseCommand { ExerciseId = exerciseId, StudentId = studentId });
        return Ok(id);
    }

    [HttpPost("{studentExerciseId}/submit")]
    public async Task<ActionResult<StudentExerciseDto>> Submit(Guid studentExerciseId, [FromBody] SubmitExerciseDto dto)
    {
        var result = await _mediator.Send(new SubmitExerciseCommand { StudentExerciseId = studentExerciseId, Answers = dto.Answers });
        return Ok(result);
    }

    [HttpGet("{studentExerciseId}")]
    public async Task<ActionResult<StudentExerciseDto>> GetResult(Guid studentExerciseId)
    {
        var result = await _mediator.Send(new GetStudentExerciseResultQuery { Id = studentExerciseId });
        return result == null ? NotFound() : Ok(result);
    }
    // 1. Xem tổng quan cả lớp
    [HttpGet("classes/{classId}/overview")]
    public async Task<ActionResult<ClassExerciseOverviewDto>> GetClassOverview(
        Guid exerciseId, Guid classId)
    {
        var result = await _mediator.Send(new GetClassExerciseOverviewQuery
        {
            ExerciseId = exerciseId,
            ClassId = classId
        });

        return result == null ? NotFound("Bài tập hoặc lớp không tồn tại") : Ok(result);
    }

    // 2. Xem chi tiết bài làm của 1 học sinh
    [HttpGet("student-exercises/{studentExerciseId}")]
    public async Task<ActionResult<StudentExerciseDetailDto>> GetStudentDetail(Guid studentExerciseId)
    {
        var result = await _mediator.Send(new GetStudentExerciseDetailQuery
        {
            StudentExerciseId = studentExerciseId
        });

        return result == null ? NotFound() : Ok(result);
    }
}