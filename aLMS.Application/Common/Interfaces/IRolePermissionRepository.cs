using aLMS.Domain.RolePermissionEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IRolePermissionRepository
    {
        Task<IEnumerable<RolePermission>> GetByRoleIdAsync(Guid roleId);
        Task<bool> ExistsAsync(Guid roleId, Guid permissionId);
        Task AddAsync(RolePermission rp);
        Task DeleteAsync(Guid roleId, Guid permissionId);
    }
}