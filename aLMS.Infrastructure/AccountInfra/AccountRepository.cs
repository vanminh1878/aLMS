using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AccountEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Username, Password, Status");
                sb.AppendLine("FROM \"Account\"");
                return await connection.QueryAsync<Account>(sb.ToString());
            }
        }

        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Username, Password, Status");
                sb.AppendLine("FROM \"Account\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Account>(sb.ToString(), new { Id = id });
            }
        }

        public async Task AddAccountAsync(Account account)
        {
            await _context.Set<Account>().AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _context.Set<Account>().Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Account\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<Account> GetAccountByUsernameAsync(string username)
        {
            return await _context.Set<Account>()
                .FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<bool> AccountExistsAsync(Guid id)
        {
            return await _context.Set<Account>().AnyAsync(a => a.Id == id);
        }
    }
}