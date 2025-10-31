// aLMS.Infrastructure.RoleInfra/RoleRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.RoleEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.RoleInfra
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public RoleRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"RoleName\" FROM \"role\"";
            return await conn.QueryAsync<Role>(sql);
        }

        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"RoleName\" FROM \"role\" WHERE \"Id\" = @id";
            return await conn.QuerySingleOrDefaultAsync<Role>(sql, new { id });
        }

        public async Task AddRoleAsync(Role role)
        {
            await _context.Set<Role>().AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            _context.Set<Role>().Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"role\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> RoleExistsAsync(Guid id)
        {
            return await _context.Set<Role>().AnyAsync(r => r.Id == id);
        }

        public async Task<bool> RoleNameExistsAsync(string roleName, Guid? excludeId = null)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = excludeId.HasValue
                ? "SELECT COUNT(1) FROM \"role\" WHERE \"RoleName\" = @roleName AND \"Id\" != @excludeId"
                : "SELECT COUNT(1) FROM \"role\" WHERE \"RoleName\" = @roleName";
            return await conn.ExecuteScalarAsync<int>(sql, new { roleName, excludeId }) > 0;
        }
    }
}