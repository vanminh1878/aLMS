using aLMS.Domain.RolePermissionEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IRolePermissionRepository
    {
        Task<IEnumerable<RolePermission>> GetAllRolePermissionsAsync();
        Task<RolePermission> GetRolePermissionByIdsAsync(Guid roleId, Guid permissionId);
        Task AddRolePermissionAsync(RolePermission rolePermission);
        Task DeleteRolePermissionAsync(Guid roleId, Guid permissionId);
        Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(Guid roleId);
        Task<bool> RolePermissionExistsAsync(Guid roleId, Guid permissionId);
    }
}