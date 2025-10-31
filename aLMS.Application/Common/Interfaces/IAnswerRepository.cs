using aLMS.Domain.AnswerEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IAnswerRepository
    {
        Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId);
        Task AddAnswerAsync(Answer answer);
        Task UpdateAnswerAsync(Answer answer);
        Task DeleteAnswerAsync(Guid id);
        Task<bool> AnswerExistsAsync(Guid id);
    }
}