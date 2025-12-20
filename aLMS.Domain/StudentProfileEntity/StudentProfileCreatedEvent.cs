using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.StudentProfileEntity
{
    public class StudentProfileCreatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid? SchoolId { get; }


        public StudentProfileCreatedEvent(Guid userId, Guid? schoolId)
        {
            UserId = userId;
            SchoolId = schoolId;
        }
    }

    public class StudentProfileUpdatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid? SchoolId { get; }

        public StudentProfileUpdatedEvent(Guid userId, Guid? schoolId)
        {
            UserId = userId;
            SchoolId = schoolId;
        }
    }

    public class StudentProfileDeletedEvent : IDomainEvent
    {
        public Guid UserId { get; }

        public StudentProfileDeletedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}