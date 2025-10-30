// aLMS.Domain.ExerciseEntity/ExerciseEvents.cs
using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.ExerciseEntity
{
    public class ExerciseCreatedEvent : IDomainEvent
    {
        public Guid ExerciseId { get; }
        public string Title { get; }
        public Guid LessonId { get; }

        public ExerciseCreatedEvent(Guid exerciseId, string title, Guid lessonId)
        {
            ExerciseId = exerciseId;
            Title = title;
            LessonId = lessonId;
        }
    }

    public class ExerciseUpdatedEvent : IDomainEvent
    {
        public Guid ExerciseId { get; }
        public string Title { get; }
        public Guid LessonId { get; }

        public ExerciseUpdatedEvent(Guid exerciseId, string title, Guid lessonId)
        {
            ExerciseId = exerciseId;
            Title = title;
            LessonId = lessonId;
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