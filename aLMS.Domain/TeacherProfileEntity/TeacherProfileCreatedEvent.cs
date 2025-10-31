// aLMS.Domain.TeacherProfileEntity/TeacherProfileEvents.cs
using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.TeacherProfileEntity
{
    public class TeacherProfileCreatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid? SchoolId { get; }
        public Guid? DepartmentId { get; }

        public TeacherProfileCreatedEvent(Guid userId, Guid? schoolId, Guid? departmentId)
        {
            UserId = userId;
            SchoolId = schoolId;
            DepartmentId = departmentId;
        }
    }

    public class TeacherProfileUpdatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid? SchoolId { get; }
        public Guid? DepartmentId { get; }

        public TeacherProfileUpdatedEvent(Guid userId, Guid? schoolId, Guid? departmentId)
        {
            UserId = userId;
            SchoolId = schoolId;
            DepartmentId = departmentId;
        }
    }

    public class TeacherProfileDeletedEvent : IDomainEvent
    {
        public Guid UserId { get; }

        public TeacherProfileDeletedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}