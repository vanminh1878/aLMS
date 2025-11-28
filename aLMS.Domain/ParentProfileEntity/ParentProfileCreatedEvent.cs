using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.ParentProfileEntity
{
    public class ParentProfileCreatedEvent : IDomainEvent
    {
        public Guid ParentId { get; }
        public Guid StudentId { get; }

        public ParentProfileCreatedEvent(Guid parentId, Guid studentId)
        {
            ParentId = parentId;
            StudentId = studentId;
        }
    }

    public class ParentProfileUpdatedEvent : IDomainEvent
    {
        public Guid ParentId { get; }
        public Guid StudentId { get; }

        public ParentProfileUpdatedEvent(Guid parentId, Guid studentId)
        {
            ParentId = parentId;
            StudentId = studentId;
        }
    }

    public class ParentProfileDeletedEvent : IDomainEvent
    {
        public Guid ParentId { get; }
        public Guid StudentId { get; }

        public ParentProfileDeletedEvent(Guid parentId, Guid studentId)
        {
            ParentId = parentId;
            StudentId = studentId;
        }
    }
}