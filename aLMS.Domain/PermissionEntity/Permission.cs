using aLMS.Domain.Common;
using aLMS.Domain.RolePermissionEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.PermissionEntity
{
    public class Permission : Entity
    {
        public string PermissionName { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public void RaisePermissionCreatedEvent() => AddDomainEvent(new PermissionCreatedEvent(Id, PermissionName));
        public void RaisePermissionUpdatedEvent() => AddDomainEvent(new PermissionUpdatedEvent(Id, PermissionName));
        public void RaisePermissionDeletedEvent() => AddDomainEvent(new PermissionDeletedEvent(Id));
    }
}
