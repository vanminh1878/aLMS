using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.SubjectEntity
{
    public class SubjectCreatedEvent : IDomainEvent
    {
        public Guid SubjectId { get; }
        public string Name { get; }
        public Guid ClassId { get; }

        public SubjectCreatedEvent(Guid subjectId, string name, Guid classId)
        {
            SubjectId = subjectId;
            Name = name;
            ClassId = classId;
        }
    }

    public class SubjectUpdatedEvent : IDomainEvent
    {
        public Guid SubjectId { get; }
        public string Name { get; }
        public Guid ClassId { get; }

        public SubjectUpdatedEvent(Guid subjectId, string name, Guid classId)
        {
            SubjectId = subjectId;
            Name = name;
            ClassId = classId;
        }
    }

    public class SubjectDeletedEvent : IDomainEvent
    {
        public Guid SubjectId { get; }

        public SubjectDeletedEvent(Guid subjectId)
        {
            SubjectId = subjectId;
        }
    }
}