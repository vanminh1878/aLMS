// aLMS.Domain.AnswerEntity/AnswerEvents.cs
using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.AnswerEntity
{
    public class AnswerCreatedEvent : IDomainEvent
    {
        public Guid AnswerId { get; }
        public string Content { get; }
        public bool IsCorrect { get; }
        public Guid QuestionId { get; }

        public AnswerCreatedEvent(Guid answerId, string content, bool isCorrect, Guid questionId)
        {
            AnswerId = answerId;
            Content = content;
            IsCorrect = isCorrect;
            QuestionId = questionId;
        }
    }

    public class AnswerUpdatedEvent : IDomainEvent
    {
        public Guid AnswerId { get; }
        public string Content { get; }
        public bool IsCorrect { get; }
        public Guid QuestionId { get; }

        public AnswerUpdatedEvent(Guid answerId, string content, bool isCorrect, Guid questionId)
        {
            AnswerId = answerId;
            Content = content;
            IsCorrect = isCorrect;
            QuestionId = questionId;
        }
    }

    public class AnswerDeletedEvent : IDomainEvent
    {
        public Guid AnswerId { get; }

        public AnswerDeletedEvent(Guid answerId)
        {
            AnswerId = answerId;
        }
    }
}