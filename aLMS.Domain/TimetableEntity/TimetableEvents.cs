using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.TimetableEntity
{
    public class TimetableCreatedEvent : IDomainEvent
    {
        public Guid Id { get; }
        public Guid ClassId { get; }
        public Guid SubjectId { get; }
        public Guid TeacherId { get; }

        public TimetableCreatedEvent(Guid id, Guid classId, Guid subjectId, Guid teacherId)
        {
            Id = id;
            ClassId = classId;
            SubjectId = subjectId;
            TeacherId = teacherId;
        }
    }

    public class TimetableUpdatedEvent : IDomainEvent
    {
        public Guid Id { get; }

        public TimetableUpdatedEvent(Guid id)
        {
            Id = id;
        }
    }

    public class TimetableDeletedEvent : IDomainEvent
    {
        public Guid Id { get; }

        public TimetableDeletedEvent(Guid id)
        {
            Id = id;
        }
    }
}