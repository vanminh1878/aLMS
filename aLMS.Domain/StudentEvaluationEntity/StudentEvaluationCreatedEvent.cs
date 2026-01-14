using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.StudentEvaluationEntity
{
    public class StudentEvaluationCreatedEvent : IDomainEvent
    {
        public Guid Id { get; }
        public Guid StudentId { get; }
        public Guid ClassId { get; }

        public StudentEvaluationCreatedEvent(Guid id, Guid studentId, Guid classId)
        {
            Id = id;
            StudentId = studentId;
            ClassId = classId;
        }
    }

    public class StudentEvaluationUpdatedEvent : IDomainEvent
    {
        public Guid Id { get; }

        public StudentEvaluationUpdatedEvent(Guid id)
        {
            Id = id;
        }
    }

    public class StudentEvaluationDeletedEvent : IDomainEvent
    {
        public Guid Id { get; }

        public StudentEvaluationDeletedEvent(Guid id)
        {
            Id = id;
        }
    }
}