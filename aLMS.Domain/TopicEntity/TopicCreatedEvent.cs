using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.TopicEntity
{
    public class TopicCreatedEvent : IDomainEvent
    {
        public Guid TopicId { get; }
        public string Title { get; }
        public Guid SubjectId { get; }

        public TopicCreatedEvent(Guid topicId, string title, Guid subjectId)
        {
            TopicId = topicId;
            Title = title;
            SubjectId = subjectId;
        }
    }

    public class TopicUpdatedEvent : IDomainEvent
    {
        public Guid TopicId { get; }
        public string Title { get; }
        public Guid SubjectId { get; }

        public TopicUpdatedEvent(Guid topicId, string title, Guid subjectId)
        {
            TopicId = topicId;
            Title = title;
            SubjectId = subjectId;
        }
    }

    public class TopicDeletedEvent : IDomainEvent
    {
        public Guid TopicId { get; }

        public TopicDeletedEvent(Guid topicId)
        {
            TopicId = topicId;
        }
    }
}