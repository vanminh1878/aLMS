using aLMS.Application.Common.DTOs;
using aLMS.Application.Statistics.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aLMS.API.Controllers
{
    [ApiController]
    [Route("api/schools/{schoolId}/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Thống kê số lượng giáo viên theo bộ môn trong trường
        /// </summary>
        [HttpGet("teachers-by-department")]
        public async Task<ActionResult<IEnumerable<TeacherCountByDepartmentDto>>> GetTeacherCountByDepartment(Guid schoolId)
        {
            var result = await _mediator.Send(new GetTeacherCountByDepartmentQuery { SchoolId = schoolId });
            return Ok(result);
        }

        /// <summary>
        /// Tỷ lệ hoàn thành bài tập về nhà của học sinh theo lớp
        /// (Tính theo tất cả exercise trong trường hoặc có thể lọc thêm sau)
        /// </summary>
        [HttpGet("exercise-completion-rate")]
        public async Task<ActionResult<IEnumerable<ExerciseCompletionRateDto>>> GetExerciseCompletionRate(Guid schoolId)
        {
            var result = await _mediator.Send(new GetExerciseCompletionRateQuery { SchoolId = schoolId });
            return Ok(result);
        }

        /// <summary>
        /// Thống kê số lượng học sinh theo khối lớp trong trường
        /// </summary>
        [HttpGet("students-by-grade")]
        public async Task<ActionResult<IEnumerable<StudentCountByGradeDto>>> GetStudentCountByGrade(Guid schoolId)
        {
            var result = await _mediator.Send(new GetStudentCountByGradeQuery { SchoolId = schoolId });
            return Ok(result);
        }
    }
}
