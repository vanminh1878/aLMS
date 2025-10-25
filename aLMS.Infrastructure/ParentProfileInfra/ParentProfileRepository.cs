using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ParentProfileEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.ParentProfileInfra
{
    public class ParentProfileRepository : IParentProfileRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public ParentProfileRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ParentProfile>> GetAllParentProfilesAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT UserId, StudentId");
                sb.AppendLine("FROM \"ParentProfile\"");
                return await connection.QueryAsync<ParentProfile>(sb.ToString());
            }
        }

        public async Task<ParentProfile> GetParentProfileByIdAsync(Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT UserId, StudentId");
                sb.AppendLine("FROM \"ParentProfile\"");
                sb.AppendLine("WHERE UserId = @UserId");
                return await connection.QuerySingleOrDefaultAsync<ParentProfile>(sb.ToString(), new { UserId = userId });
            }
        }

        public async Task AddParentProfileAsync(ParentProfile parentProfile)
        {
            await _context.Set<ParentProfile>().AddAsync(parentProfile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateParentProfileAsync(ParentProfile parentProfile)
        {
            _context.Set<ParentProfile>().Update(parentProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteParentProfileAsync(Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"ParentProfile\"");
                sb.AppendLine("WHERE UserId = @UserId");
                await connection.ExecuteAsync(sb.ToString(), new { UserId = userId });
            }
        }

        public async Task<IEnumerable<ParentProfile>> GetParentProfilesByStudentIdAsync(Guid studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT UserId, StudentId");
                sb.AppendLine("FROM \"ParentProfile\"");
                sb.AppendLine("WHERE StudentId = @StudentId");
                return await connection.QueryAsync<ParentProfile>(sb.ToString(), new { StudentId = studentId });
            }
        }

        public async Task<bool> ParentProfileExistsAsync(Guid userId)
        {
            return await _context.Set<ParentProfile>().AnyAsync(pp => pp.UserId == userId);
        }
    }
}