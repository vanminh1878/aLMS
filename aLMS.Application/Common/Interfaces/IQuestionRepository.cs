using aLMS.Domain.QuestionEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<Question> GetQuestionByIdAsync(Guid id);
        Task<IEnumerable<Question>> GetQuestionsByExerciseIdAsync(Guid exerciseId);
        Task AddQuestionAsync(Question question);
        Task UpdateQuestionAsync(Question question);
        Task DeleteQuestionAsync(Guid id);
        Task<bool> QuestionExistsAsync(Guid id);
    }
}