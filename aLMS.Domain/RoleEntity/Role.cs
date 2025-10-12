using aLMS.Domain.Common;
using aLMS.Domain.RolePermissionEntity;
using aLMS.Domain.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.RoleEntity
{
    public class Role : Entity
    {
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }

}
