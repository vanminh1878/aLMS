using aLMS.Domain.StudentAnswerEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStudentAnswerRepository
    {
        Task<IEnumerable<StudentAnswer>> GetAllStudentAnswersAsync();
        Task<StudentAnswer> GetStudentAnswerByIdAsync(Guid id);
        Task AddStudentAnswerAsync(StudentAnswer studentAnswer);
        Task UpdateStudentAnswerAsync(StudentAnswer studentAnswer);
        Task DeleteStudentAnswerAsync(Guid id);
        Task<IEnumerable<StudentAnswer>> GetStudentAnswersByStudentExerciseIdAsync(Guid studentExerciseId);
        Task<bool> StudentAnswerExistsAsync(Guid id);
    }
}