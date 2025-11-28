using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.RolePermissionEntity
{
    public class RolePermissionAddedEvent : IDomainEvent
    {
        public Guid RoleId { get; }
        public Guid PermissionId { get; }

        public RolePermissionAddedEvent(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }

    public class RolePermissionRemovedEvent : IDomainEvent
    {
        public Guid RoleId { get; }
        public Guid PermissionId { get; }

        public RolePermissionRemovedEvent(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}