using aLMS.Domain.Common;
using aLMS.Domain.SubjectEntity;
using System;

namespace aLMS.Domain.ExerciseEntity
{
    public class ExerciseCreatedEvent : IDomainEvent
    {
        public Guid ExerciseId { get; }
        public string Title { get; }
        public Guid TopicId { get; }

        public ExerciseCreatedEvent(Guid exerciseId, string title, Guid topicId)
        {
            ExerciseId = exerciseId;
            Title = title;
            TopicId = topicId;
        }
    }

    public class ExerciseUpdatedEvent : IDomainEvent
    {
        public Guid ExerciseId { get; }
        public string Title { get; }
        public Guid TopicId { get; }

        public ExerciseUpdatedEvent(Guid exerciseId, string title, Guid topicId)
        {
            ExerciseId = exerciseId;
            Title = title;
            TopicId = topicId;
        }
    }

    public class ExerciseDeletedEvent : IDomainEvent
    {
        public Guid ExerciseId { get; }

        public ExerciseDeletedEvent(Guid exerciseId)
        {
            ExerciseId = exerciseId;
        }
    }
}