// aLMS.Infrastructure.AccountInfra/AccountRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AccountEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.AccountInfra
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public AccountRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"Username\", \"PasswordHash\", \"Status\" FROM \"account\" WHERE \"Username\" = @username";
            return await conn.QuerySingleOrDefaultAsync<Account>(sql, new { username });
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"Username\", \"PasswordHash\", \"Status\" FROM \"account\" WHERE \"Id\" = @id";
            return await conn.QuerySingleOrDefaultAsync<Account>(sql, new { id });
        }

        public async Task AddAsync(Account account)
        {
            await _context.Set<Account>().AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Set<Account>().Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"account\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> UsernameExistsAsync(string username, Guid? excludeId = null)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = excludeId.HasValue
                ? "SELECT COUNT(1) FROM \"account\" WHERE \"Username\" = @username AND \"Id\" != @excludeId"
                : "SELECT COUNT(1) FROM \"account\" WHERE \"Username\" = @username";
            return await conn.ExecuteScalarAsync<int>(sql, new { username, excludeId }) > 0;
        }
    }
}