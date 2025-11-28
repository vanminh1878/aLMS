using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.StudentAnswerEntity
{
    public class StudentAnswerSubmittedEvent : IDomainEvent
    {
        public Guid StudentAnswerId { get; }
        public Guid StudentExerciseId { get; }
        public Guid QuestionId { get; }
        public Guid? AnswerId { get; }
        public string? AnswerText { get; }
        public bool IsCorrect { get; }
        public DateTime SubmittedAt { get; }

        public StudentAnswerSubmittedEvent(
            Guid studentAnswerId,
            Guid studentExerciseId,
            Guid questionId,
            Guid? answerId,
            string? answerText,
            bool isCorrect,
            DateTime submittedAt)
        {
            StudentAnswerId = studentAnswerId;
            StudentExerciseId = studentExerciseId;
            QuestionId = questionId;
            AnswerId = answerId;
            AnswerText = answerText;
            IsCorrect = isCorrect;
            SubmittedAt = submittedAt;
        }
    }
}