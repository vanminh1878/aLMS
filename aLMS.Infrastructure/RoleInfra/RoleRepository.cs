using aLMS.Application.Common.Interfaces;
using aLMS.Domain.RoleEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, RoleName");
                sb.AppendLine("FROM \"Role\"");
                return await connection.QueryAsync<Role>(sb.ToString());
            }
        }

        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, RoleName");
                sb.AppendLine("FROM \"Role\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Role>(sb.ToString(), new { Id = id });
            }
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Role\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<bool> RoleExistsAsync(Guid id)
        {
            return await _context.Set<Role>().AnyAsync(r => r.Id == id);
        }
    }
}