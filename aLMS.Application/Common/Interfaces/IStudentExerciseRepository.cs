using aLMS.Domain.StudentExerciseEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStudentExerciseRepository
    {
        Task<StudentExercise?> GetActiveByStudentAndExerciseAsync(Guid studentId, Guid exerciseId);
        Task<StudentExercise?> GetByIdAsync(Guid id);
        Task AddAsync(StudentExercise se);
        Task UpdateAsync(StudentExercise se);
    }
}