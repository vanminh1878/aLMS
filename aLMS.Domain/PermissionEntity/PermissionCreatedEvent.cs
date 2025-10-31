// aLMS.Domain.PermissionEntity/PermissionEvents.cs
using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.PermissionEntity
{
    public class PermissionCreatedEvent : IDomainEvent
    {
        public Guid PermissionId { get; }
        public string PermissionName { get; }

        public PermissionCreatedEvent(Guid permissionId, string permissionName)
        {
            PermissionId = permissionId;
            PermissionName = permissionName;
        }
    }

    public class PermissionUpdatedEvent : IDomainEvent
    {
        public Guid PermissionId { get; }
        public string PermissionName { get; }

        public PermissionUpdatedEvent(Guid permissionId, string permissionName)
        {
            PermissionId = permissionId;
            PermissionName = permissionName;
        }
    }

    public class PermissionDeletedEvent : IDomainEvent
    {
        public Guid PermissionId { get; }

        public PermissionDeletedEvent(Guid permissionId)
        {
            PermissionId = permissionId;
        }
    }
}