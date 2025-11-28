using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.LessonEntity
{
    public class LessonCreatedEvent : IDomainEvent
    {
        public Guid LessonId { get; }
        public string Title { get; }
        public Guid TopicId { get; }

        public LessonCreatedEvent(Guid lessonId, string title, Guid topicId)
        {
            LessonId = lessonId;
            Title = title;
            TopicId = topicId;
        }
    }

    public class LessonUpdatedEvent : IDomainEvent
    {
        public Guid LessonId { get; }
        public string Title { get; }
        public Guid TopicId { get; }

        public LessonUpdatedEvent(Guid lessonId, string title, Guid topicId)
        {
            LessonId = lessonId;
            Title = title;
            TopicId = topicId;
        }
    }

    public class LessonDeletedEvent : IDomainEvent
    {
        public Guid LessonId { get; }

        public LessonDeletedEvent(Guid lessonId)
        {
            LessonId = lessonId;
        }
    }
}