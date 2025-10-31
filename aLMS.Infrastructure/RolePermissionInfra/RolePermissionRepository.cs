// aLMS.Infrastructure.RolePermissionInfra/RolePermissionRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.PermissionEntity;
using aLMS.Domain.RoleEntity;
using aLMS.Domain.RolePermissionEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<RolePermission>> GetByRoleIdAsync(Guid roleId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT rp.*, r.""RoleName"", p.""PermissionName""
                FROM ""role_permission"" rp
                JOIN ""role"" r ON rp.""RoleId"" = r.""Id""
                JOIN ""permission"" p ON rp.""PermissionId"" = p.""Id""
                WHERE rp.""RoleId"" = @roleId";
            return await conn.QueryAsync<RolePermission, Role, Permission, RolePermission>(
                sql,
                (rp, role, perm) =>
                {
                    rp.Role = role;
                    rp.Permission = perm;
                    return rp;
                },
                new { roleId },
                splitOn: "Id,Id"
            );
        }

        public async Task<bool> ExistsAsync(Guid roleId, Guid permissionId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM \"role_permission\" WHERE \"RoleId\" = @roleId AND \"PermissionId\" = @permissionId";
            return await conn.ExecuteScalarAsync<int>(sql, new { roleId, permissionId }) > 0;
        }

        public async Task AddAsync(RolePermission rp)
        {
            await _context.Set<RolePermission>().AddAsync(rp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid roleId, Guid permissionId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"role_permission\" WHERE \"RoleId\" = @roleId AND \"PermissionId\" = @permissionId";
            await conn.ExecuteAsync(sql, new { roleId, permissionId });
        }
    }
}