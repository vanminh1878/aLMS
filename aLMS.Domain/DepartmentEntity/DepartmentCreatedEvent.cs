using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.DepartmentEntity
{
    public class DepartmentCreatedEvent : IDomainEvent
    {
        public Guid DepartmentId { get; }
        public string DepartmentName { get; }

        public DepartmentCreatedEvent(Guid departmentId, string departmentName)
        {
            DepartmentId = departmentId;
            DepartmentName = departmentName;
        }
    }

    public class DepartmentUpdatedEvent : IDomainEvent
    {
        public Guid DepartmentId { get; }
        public string DepartmentName { get; }

        public DepartmentUpdatedEvent(Guid departmentId, string departmentName)
        {
            DepartmentId = departmentId;
            DepartmentName = departmentName;
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