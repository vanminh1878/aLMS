// aLMS.Infrastructure.StudentProfileInfra/StudentProfileRepository.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassEntity;
using aLMS.Domain.StudentProfileEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
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
                SELECT 
                    sp.*,
                    u.""Name"" AS UserName,
                    u.""Email"",
                    s.""Name"" AS SchoolName,
                    c.""ClassName""
                FROM ""student_profile"" sp
                LEFT JOIN ""user"" u ON sp.""UserId"" = u.""Id""
                LEFT JOIN ""school"" s ON sp.""SchoolId"" = s.""Id""
                LEFT JOIN ""student_class_enrollment"" sce ON sp.""UserId"" = sce.""StudentProfileId""
                LEFT JOIN ""class"" c ON sce.""ClassId"" = c.""Id""
                WHERE sp.""UserId"" = @userId
                ";  

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
                FROM ""student_class_enrollment""
                WHERE ""ClassId"" = @classId";

            return await conn.ExecuteScalarAsync<int>(sql, new { classId });
        }
        public async Task<List<StudentProfileDto>> GetByClassIdAsync(Guid classId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT 
                    sp.*,
                    u.""Name"" AS UserName,
                    u.""Email"",
                    s.""Name"" AS SchoolName,
                    c.""ClassName""
                FROM ""student_class_enrollment"" sce
                JOIN ""student_profile"" sp ON sce.""StudentProfileId"" = sp.""UserId""
                LEFT JOIN ""user"" u ON sp.""UserId"" = u.""Id""
                LEFT JOIN ""school"" s ON sp.""SchoolId"" = s.""Id""
                LEFT JOIN ""class"" c ON sce.""ClassId"" = c.""Id""
                WHERE sce.""ClassId"" = @classId
                ORDER BY u.""Name""";

            var result = await conn.QueryAsync<StudentProfileDto>(sql, new { classId });
            return result.ToList();
        }
    }
}