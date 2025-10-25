using aLMS.Domain.StudentExerciseEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStudentExerciseRepository
    {
        Task<IEnumerable<StudentExercise>> GetAllStudentExercisesAsync();
        Task<StudentExercise> GetStudentExerciseByIdAsync(Guid id);
        Task AddStudentExerciseAsync(StudentExercise studentExercise);
        Task UpdateStudentExerciseAsync(StudentExercise studentExercise);
        Task DeleteStudentExerciseAsync(Guid id);
        Task<IEnumerable<StudentExercise>> GetStudentExercisesByStudentIdAsync(Guid studentId);
        Task<bool> StudentExerciseExistsAsync(Guid id);
    }
}