using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.QuestionEntity
{
    public class QuestionCreatedEvent : IDomainEvent
    {
        public Guid QuestionId { get; }
        public string Content { get; }
        public Guid ExerciseId { get; }

        public QuestionCreatedEvent(Guid questionId, string content, Guid exerciseId)
        {
            QuestionId = questionId;
            Content = content;
            ExerciseId = exerciseId;
        }
    }

    public class QuestionUpdatedEvent : IDomainEvent
    {
        public Guid QuestionId { get; }
        public string Content { get; }
        public Guid ExerciseId { get; }

        public QuestionUpdatedEvent(Guid questionId, string content, Guid exerciseId)
        {
            QuestionId = questionId;
            Content = content;
            ExerciseId = exerciseId;
        }
    }

    public class QuestionDeletedEvent : IDomainEvent
    {
        public Guid QuestionId { get; }

        public QuestionDeletedEvent(Guid questionId)
        {
            QuestionId = questionId;
        }
    }
}