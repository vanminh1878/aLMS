// aLMS.Domain.DepartmentEntity/DepartmentEvents.cs
using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.DepartmentEntity
{
    public class DepartmentCreatedEvent : IDomainEvent
    {
        public Guid DepartmentId { get; }
        public string DepartmentName { get; }
        public Guid SchoolId { get; }

        public DepartmentCreatedEvent(Guid departmentId, string departmentName, Guid schoolId)
        {
            DepartmentId = departmentId;
            DepartmentName = departmentName;
            SchoolId = schoolId;
        }
    }

    public class DepartmentUpdatedEvent : IDomainEvent
    {
        public Guid DepartmentId { get; }
        public string DepartmentName { get; }
        public Guid SchoolId { get; }

        public DepartmentUpdatedEvent(Guid departmentId, string departmentName, Guid schoolId)
        {
            DepartmentId = departmentId;
            DepartmentName = departmentName;
            SchoolId = schoolId;
        }
    }

    public class DepartmentDeletedEvent : IDomainEvent
    {
        public Guid DepartmentId { get; }

        public DepartmentDeletedEvent(Guid departmentId)
        {
            DepartmentId = departmentId;
        }
    }
}