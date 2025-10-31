using aLMS.Domain.StudentAnswerEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStudentAnswerRepository
    {
        Task AddRangeAsync(IEnumerable<StudentAnswer> answers);
        Task<IEnumerable<StudentAnswer>> GetByStudentExerciseIdAsync(Guid studentExerciseId);
    }
}
