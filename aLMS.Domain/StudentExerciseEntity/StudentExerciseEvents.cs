using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.StudentExerciseEntity
{
    public class StudentExerciseStartedEvent : IDomainEvent
    {
        public Guid StudentExerciseId { get; }
        public Guid StudentId { get; }
        public Guid ExerciseId { get; }

        public StudentExerciseStartedEvent(Guid studentExerciseId, Guid studentId, Guid exerciseId)
        {
            StudentExerciseId = studentExerciseId;
            StudentId = studentId;
            ExerciseId = exerciseId;
        }
    }

    public class StudentExerciseSubmittedEvent : IDomainEvent
    {
        public Guid StudentExerciseId { get; }
        public decimal Score { get; }
        public bool IsCompleted { get; }
        public DateTime? EndTime { get; }

        public StudentExerciseSubmittedEvent(Guid studentExerciseId, decimal score, bool isCompleted, DateTime? endTime)
        {
            StudentExerciseId = studentExerciseId;
            Score = score;
            IsCompleted = isCompleted;
            EndTime = endTime;
        }
    }
}