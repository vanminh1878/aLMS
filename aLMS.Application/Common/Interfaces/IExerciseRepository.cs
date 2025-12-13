using aLMS.Domain.ExerciseEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IExerciseRepository
    {
        Task<IEnumerable<Exercise>> GetAllExercisesAsync();
        Task<Exercise> GetExerciseByIdAsync(Guid id);
        Task<IEnumerable<Exercise>> GetExercisesByTopicIdAsync(Guid lessonId);
        Task AddExerciseAsync(Exercise exercise);
        Task UpdateExerciseAsync(Exercise exercise);
        Task DeleteExerciseAsync(Guid id);
        Task<bool> ExerciseExistsAsync(Guid id);
    }
}