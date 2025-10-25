using aLMS.Application.Common.Interfaces;
using aLMS.Domain.TeacherProfileEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.TeacherProfileInfra
{
    public class TeacherProfileRepository : ITeacherProfileRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public TeacherProfileRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TeacherProfile>> GetAllTeacherProfilesAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT UserId, SchoolId, DepartmentId, HireDate, Specialization");
                sb.AppendLine("FROM \"TeacherProfile\"");
                return await connection.QueryAsync<TeacherProfile>(sb.ToString());
            }
        }

        public async Task<TeacherProfile> GetTeacherProfileByIdAsync(Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT UserId, SchoolId, DepartmentId, HireDate, Specialization");
                sb.AppendLine("FROM \"TeacherProfile\"");
                sb.AppendLine("WHERE UserId = @UserId");
                return await connection.QuerySingleOrDefaultAsync<TeacherProfile>(sb.ToString(), new { UserId = userId });
            }
        }

        public async Task AddTeacherProfileAsync(TeacherProfile teacherProfile)
        {
            await _context.Set<TeacherProfile>().AddAsync(teacherProfile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTeacherProfileAsync(TeacherProfile teacherProfile)
        {
            _context.Set<TeacherProfile>().Update(teacherProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTeacherProfileAsync(Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"TeacherProfile\"");
                sb.AppendLine("WHERE UserId = @UserId");
                await connection.ExecuteAsync(sb.ToString(), new { UserId = userId });
            }
        }

        public async Task<IEnumerable<TeacherProfile>> GetTeacherProfilesBySchoolIdAsync(Guid schoolId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT UserId, SchoolId, DepartmentId, HireDate, Specialization");
                sb.AppendLine("FROM \"TeacherProfile\"");
                sb.AppendLine("WHERE SchoolId = @SchoolId");
                return await connection.QueryAsync<TeacherProfile>(sb.ToString(), new { SchoolId = schoolId });
            }
        }

        public async Task<bool> TeacherProfileExistsAsync(Guid userId)
        {
            return await _context.Set<TeacherProfile>().AnyAsync(tp => tp.UserId == userId);
        }
    }
}