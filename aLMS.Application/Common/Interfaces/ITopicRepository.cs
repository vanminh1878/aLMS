using aLMS.Domain.TopicEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAllTopicsAsync();
        Task<Topic> GetTopicByIdAsync(Guid id);
        Task AddTopicAsync(Topic topic);
        Task UpdateTopicAsync(Topic topic);
        Task DeleteTopicAsync(Guid id);
        Task<IEnumerable<Topic>> GetTopicsBySubjectIdAsync(Guid subjectId);
        Task<bool> TopicExistsAsync(Guid id);
    }
}