// aLMS.Domain.StudentProfileEntity/StudentProfileEvents.cs
using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.StudentProfileEntity
{
    public class StudentProfileCreatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid? SchoolId { get; }
        public Guid? ClassId { get; }

        public StudentProfileCreatedEvent(Guid userId, Guid? schoolId, Guid? classId)
        {
            UserId = userId;
            SchoolId = schoolId;
            ClassId = classId;
        }
    }

    public class StudentProfileUpdatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid? SchoolId { get; }
        public Guid? ClassId { get; }

        public StudentProfileUpdatedEvent(Guid userId, Guid? schoolId, Guid? classId)
        {
            UserId = userId;
            SchoolId = schoolId;
            ClassId = classId;
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