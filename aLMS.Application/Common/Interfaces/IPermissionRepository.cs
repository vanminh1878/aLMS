using aLMS.Domain.PermissionEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
        Task<Permission> GetPermissionByIdAsync(Guid id);
        Task AddPermissionAsync(Permission permission);
        Task UpdatePermissionAsync(Permission permission);
        Task DeletePermissionAsync(Guid id);
        Task<bool> PermissionExistsAsync(Guid id);
        Task<bool> PermissionNameExistsAsync(string name, Guid? excludeId = null);
    }
}