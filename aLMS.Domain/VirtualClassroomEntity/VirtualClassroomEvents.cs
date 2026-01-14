using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.VirtualClassroomEntity
{
    public class VirtualClassroomCreatedEvent : IDomainEvent
    {
        public Guid Id { get; }
        public Guid ClassId { get; }
        public string Title { get; }

        public VirtualClassroomCreatedEvent(Guid id, Guid classId, string title)
        {
            Id = id;
            ClassId = classId;
            Title = title;
        }
    }

    public class VirtualClassroomUpdatedEvent : IDomainEvent
    {
        public Guid Id { get; }

        public VirtualClassroomUpdatedEvent(Guid id)
        {
            Id = id;
        }
    }

    public class VirtualClassroomDeletedEvent : IDomainEvent
    {
        public Guid Id { get; }

        public VirtualClassroomDeletedEvent(Guid id)
        {
            Id = id;
        }
    }
}