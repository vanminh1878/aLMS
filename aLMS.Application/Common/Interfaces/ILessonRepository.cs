using aLMS.Domain.LessonEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface ILessonRepository
    {
        Task<IEnumerable<Lesson>> GetAllLessonsAsync();
        Task<Lesson> GetLessonByIdAsync(Guid id);
        Task<IEnumerable<Lesson>> GetLessonsByTopicIdAsync(Guid topicId);
        Task AddLessonAsync(Lesson lesson);
        Task UpdateLessonAsync(Lesson lesson);
        Task DeleteLessonAsync(Guid id);
        Task<bool> LessonExistsAsync(Guid id);
    }
}