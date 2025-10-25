using aLMS.Application.Common.Interfaces;
using aLMS.Domain.PermissionEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, PermissionName");
                sb.AppendLine("FROM \"Permission\"");
                return await connection.QueryAsync<Permission>(sb.ToString());
            }
        }

        public async Task<Permission> GetPermissionByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, PermissionName");
                sb.AppendLine("FROM \"Permission\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Permission>(sb.ToString(), new { Id = id });
            }
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Permission\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<bool> PermissionExistsAsync(Guid id)
        {
            return await _context.Set<Permission>().AnyAsync(p => p.Id == id);
        }
    }
}