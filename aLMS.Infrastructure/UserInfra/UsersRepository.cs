using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AccountEntity;
using aLMS.Domain.RoleEntity;
using aLMS.Domain.SchoolEntity;
using aLMS.Domain.UserEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Name, DateOfBirth, Gender, PhoneNumber, Email, Address, SchoolId, AccountId, RoleId");
                sb.AppendLine("FROM \"User\"");
                return await connection.QueryAsync<User>(sb.ToString());
            }
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Name, DateOfBirth, Gender, PhoneNumber, Email, Address, SchoolId, AccountId, RoleId");
                sb.AppendLine("FROM \"User\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<User>(sb.ToString(), new { Id = id });
            }
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Set<User>().Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"User\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsersBySchoolIdAsync(Guid schoolId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Name, DateOfBirth, Gender, PhoneNumber, Email, Address, SchoolId, AccountId, RoleId");
                sb.AppendLine("FROM \"User\"");
                sb.AppendLine("WHERE SchoolId = @SchoolId");
                return await connection.QueryAsync<User>(sb.ToString(), new { SchoolId = schoolId });
            }
        }

        public async Task<bool> UserExistsAsync(Guid id)
        {
            return await _context.Set<User>().AnyAsync(u => u.Id == id);
        }
    }
}