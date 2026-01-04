using aLMS.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<IEnumerable<TeacherCountByDepartmentDto>> GetTeacherCountByDepartmentAsync(Guid schoolId);

        Task<IEnumerable<ExerciseCompletionRateDto>> GetExerciseCompletionRateByClassAsync(Guid schoolId);

        Task<IEnumerable<StudentCountByGradeDto>> GetStudentCountByGradeAsync(Guid schoolId);
    }
}
