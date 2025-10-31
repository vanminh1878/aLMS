// aLMS.Infrastructure.PermissionInfra/PermissionRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.PermissionEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.PermissionInfra
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public PermissionRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"PermissionName\" FROM \"permission\"";
            return await conn.QueryAsync<Permission>(sql);
        }

        public async Task<Permission> GetPermissionByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"PermissionName\" FROM \"permission\" WHERE \"Id\" = @id";
            return await conn.QuerySingleOrDefaultAsync<Permission>(sql, new { id });
        }

        public async Task AddPermissionAsync(Permission permission)
        {
            await _context.Set<Permission>().AddAsync(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePermissionAsync(Permission permission)
        {
            _context.Set<Permission>().Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePermissionAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"permission\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> PermissionExistsAsync(Guid id)
        {
            return await _context.Set<Permission>().AnyAsync(p => p.Id == id);
        }

        public async Task<bool> PermissionNameExistsAsync(string name, Guid? excludeId = null)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = excludeId.HasValue
                ? "SELECT COUNT(1) FROM \"permission\" WHERE \"PermissionName\" = @name AND \"Id\" != @excludeId"
                : "SELECT COUNT(1) FROM \"permission\" WHERE \"PermissionName\" = @name";
            return await conn.ExecuteScalarAsync<int>(sql, new { name, excludeId }) > 0;
        }
    }
}