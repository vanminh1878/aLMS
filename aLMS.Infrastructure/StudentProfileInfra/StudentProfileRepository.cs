// aLMS.Infrastructure.StudentProfileInfra/StudentProfileRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentProfileEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
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

        public async Task<StudentProfile?> GetByUserIdAsync(Guid userId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT sp.*, u.""Name"" as UserName, u.""Email"", 
                       s.""Name"" as SchoolName, c.""ClassName""
                FROM ""student_profile"" sp
                LEFT JOIN ""user"" u ON sp.""UserId"" = u.""Id""
                LEFT JOIN ""school"" s ON sp.""SchoolId"" = s.""Id""
                LEFT JOIN ""class"" c ON sp.""ClassId"" = c.""Id""
                WHERE sp.""UserId"" = @userId";
            return await conn.QuerySingleOrDefaultAsync<StudentProfile>(sql, new { userId });
        }

        public async Task AddAsync(StudentProfile profile)
        {
            await _context.Set<StudentProfile>().AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentProfile profile)
        {
            _context.Set<StudentProfile>().Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"student_profile\" WHERE \"UserId\" = @userId";
            await conn.ExecuteAsync(sql, new { userId });
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM \"student_profile\" WHERE \"UserId\" = @userId";
            return await conn.ExecuteScalarAsync<int>(sql, new { userId }) > 0;
        }
        public async Task<int> GetMaxStudentOrderInClass(Guid classId)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"
        SELECT COUNT(*) 
        FROM ""student_profile"" sp
        WHERE sp.""ClassId"" = @classId";

            return await conn.ExecuteScalarAsync<int>(sql, new { classId });
        }

        public async Task<List<StudentProfile>> GetByClassIdAsync(Guid classId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        SELECT sp.*, 
               u.""Name"" as UserName, 
               u.""Email"",
               s.""Name"" as SchoolName, 
               c.""ClassName""
        FROM ""student_profile"" sp
        LEFT JOIN ""user"" u ON sp.""UserId"" = u.""Id""
        LEFT JOIN ""school"" s ON sp.""SchoolId"" = s.""Id""
        LEFT JOIN ""class"" c ON sp.""ClassId"" = c.""Id""
        WHERE sp.""ClassId"" = @classId
        ORDER BY u.""Name""";

            var result = await conn.QueryAsync<StudentProfile>(sql, new { classId });
            return result.ToList();
        }
    }

}