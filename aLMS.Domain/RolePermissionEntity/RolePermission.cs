using aLMS.Domain.Common;
using aLMS.Domain.PermissionEntity;
using aLMS.Domain.RoleEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.RolePermissionEntity
{
    public class RolePermission : Entity
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
