using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.RoleEntity
{
    public class RoleCreatedEvent : IDomainEvent
    {
        public Guid RoleId { get; }
        public string RoleName { get; }

        public RoleCreatedEvent(Guid roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }
    }

    public class RoleUpdatedEvent : IDomainEvent
    {
        public Guid RoleId { get; }
        public string RoleName { get; }

        public RoleUpdatedEvent(Guid roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }
    }

    public class RoleDeletedEvent : IDomainEvent
    {
        public Guid RoleId { get; }

        public RoleDeletedEvent(Guid roleId)
        {
            RoleId = roleId;
        }
    }
}