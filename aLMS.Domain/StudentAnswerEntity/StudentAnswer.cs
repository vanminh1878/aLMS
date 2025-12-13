using aLMS.Domain.AnswerEntity;
using aLMS.Domain.Common;
using aLMS.Domain.QuestionEntity;
using aLMS.Domain.StudentExerciseEntity;
using aLMS.Domain.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.StudentAnswerEntity
{
    public class StudentAnswer : Entity
    {
        public Guid StudentExerciseId { get; set; }
        public StudentExercise StudentExercise { get; set; }

        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public Guid AnswerId { get; set; }
        public Answer Answer { get; set; }

        public string? AnswerText { get; set; }
        public bool IsCorrect { get; set; } = false;
        public DateTime SubmittedAt { get; set; }
        public void RaiseSubmittedEvent()
        {
            AddDomainEvent(new StudentAnswerSubmittedEvent(
                Id,
                StudentExerciseId,
                QuestionId,
                AnswerId,
                AnswerText,
                IsCorrect,
                SubmittedAt
            ));
        }

    }
}
