using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentProfileEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentProfileInfra
{
    public class StudentProfileRepository : IStudentProfileRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public StudentProfileRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<StudentProfile>> GetAllStudentProfilesAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT UserId, SchoolId, ClassId, EnrollDate");
                sb.AppendLine("FROM \"StudentProfile\"");
                return await connection.QueryAsync<StudentProfile>(sb.ToString());
            }
        }

        public async Task<StudentProfile> GetStudentProfileByIdAsync(Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT UserId, SchoolId, ClassId, EnrollDate");
                sb.AppendLine("FROM \"StudentProfile\"");
                sb.AppendLine("WHERE UserId = @UserId");
                return await connection.QuerySingleOrDefaultAsync<StudentProfile>(sb.ToString(), new { UserId = userId });
            }
        }

        public async Task AddStudentProfileAsync(StudentProfile studentProfile)
        {
            await _context.Set<StudentProfile>().AddAsync(studentProfile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStudentProfileAsync(StudentProfile studentProfile)
        {
            _context.Set<StudentProfile>().Update(studentProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentProfileAsync(Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"StudentProfile\"");
                sb.AppendLine("WHERE UserId = @UserId");
                await connection.ExecuteAsync(sb.ToString(), new { UserId = userId });
            }
        }

        public async Task<IEnumerable<StudentProfile>> GetStudentProfilesBySchoolIdAsync(Guid schoolId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT UserId, SchoolId, ClassId, EnrollDate");
                sb.AppendLine("FROM \"StudentProfile\"");
                sb.AppendLine("WHERE SchoolId = @SchoolId");
                return await connection.QueryAsync<StudentProfile>(sb.ToString(), new { SchoolId = schoolId });
            }
        }

        public async Task<bool> StudentProfileExistsAsync(Guid userId)
        {
            return await _context.Set<StudentProfile>().AnyAsync(sp => sp.UserId == userId);
        }
    }
}