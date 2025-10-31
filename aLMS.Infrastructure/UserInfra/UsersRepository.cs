// aLMS.Infrastructure.UserInfra/UserRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.UserEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.UserInfra
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public UsersRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT u.*, a.""Username"", r.""RoleName"", s.""Name"" as SchoolName
                FROM ""user"" u
                LEFT JOIN ""account"" a ON u.""AccountId"" = a.""Id""
                LEFT JOIN ""role"" r ON u.""RoleId"" = r.""Id""
                LEFT JOIN ""school"" s ON u.""SchoolId"" = s.""Id""
                WHERE u.""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { id });
        }

        public async Task<User?> GetByAccountIdAsync(Guid accountId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT * FROM \"user\" WHERE \"AccountId\" = @accountId";
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { accountId });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT u.*, a.""Username"", r.""RoleName"", s.""Name"" as SchoolName
                FROM ""user"" u
                LEFT JOIN ""account"" a ON u.""AccountId"" = a.""Id""
                LEFT JOIN ""role"" r ON u.""RoleId"" = r.""Id""
                LEFT JOIN ""school"" s ON u.""SchoolId"" = s.""Id""";
            return await conn.QueryAsync<User>(sql);
        }

        public async Task AddAsync(User user)
        {
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Set<User>().Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"user\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = excludeId.HasValue
                ? "SELECT COUNT(1) FROM \"user\" WHERE \"Email\" = @email AND \"Id\" != @excludeId"
                : "SELECT COUNT(1) FROM \"user\" WHERE \"Email\" = @email";
            return await conn.ExecuteScalarAsync<int>(sql, new { email, excludeId }) > 0;
        }
    }
}