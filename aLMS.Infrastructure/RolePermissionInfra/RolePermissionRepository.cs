using aLMS.Application.Common.Interfaces;
using aLMS.Domain.RolePermissionEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.RolePermissionInfra
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public RolePermissionRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<RolePermission>> GetAllRolePermissionsAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT RoleId, PermissionId");
                sb.AppendLine("FROM \"RolePermission\"");
                return await connection.QueryAsync<RolePermission>(sb.ToString());
            }
        }

        public async Task<RolePermission> GetRolePermissionByIdsAsync(Guid roleId, Guid permissionId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT RoleId, PermissionId");
                sb.AppendLine("FROM \"RolePermission\"");
                sb.AppendLine("WHERE RoleId = @RoleId AND PermissionId = @PermissionId");
                return await connection.QuerySingleOrDefaultAsync<RolePermission>(sb.ToString(), new { RoleId = roleId, PermissionId = permissionId });
            }
        }

        public async Task AddRolePermissionAsync(RolePermission rolePermission)
        {
            await _context.Set<RolePermission>().AddAsync(rolePermission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRolePermissionAsync(Guid roleId, Guid permissionId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"RolePermission\"");
                sb.AppendLine("WHERE RoleId = @RoleId AND PermissionId = @PermissionId");
                await connection.ExecuteAsync(sb.ToString(), new { RoleId = roleId, PermissionId = permissionId });
            }
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(Guid roleId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT RoleId, PermissionId");
                sb.AppendLine("FROM \"RolePermission\"");
                sb.AppendLine("WHERE RoleId = @RoleId");
                return await connection.QueryAsync<RolePermission>(sb.ToString(), new { RoleId = roleId });
            }
        }

        public async Task<bool> RolePermissionExistsAsync(Guid roleId, Guid permissionId)
        {
            return await _context.Set<RolePermission>()
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        }
    }
}