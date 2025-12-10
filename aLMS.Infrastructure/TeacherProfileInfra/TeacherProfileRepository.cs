// aLMS.Infrastructure.TeacherProfileInfra/TeacherProfileRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.TeacherProfileEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
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

        public async Task<TeacherProfile?> GetByUserIdAsync(Guid userId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT tp.*, u.""Name"" as UserName, u.""Email"",
                       s.""Name"" as SchoolName, d.""DepartmentName""
                FROM ""teacher_profile"" tp
                LEFT JOIN ""user"" u ON tp.""UserId"" = u.""Id""
                LEFT JOIN ""school"" s ON tp.""SchoolId"" = s.""Id""
                LEFT JOIN ""department"" d ON tp.""DepartmentId"" = d.""Id""
                WHERE tp.""UserId"" = @userId";
            return await conn.QuerySingleOrDefaultAsync<TeacherProfile>(sql, new { userId });
        }

        public async Task AddAsync(TeacherProfile profile)
        {
            await _context.Set<TeacherProfile>().AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TeacherProfile profile)
        {
            _context.Set<TeacherProfile>().Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"teacher_profile\" WHERE \"UserId\" = @userId";
            await conn.ExecuteAsync(sql, new { userId });
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM \"teacher_profile\" WHERE \"UserId\" = @userId";
            return await conn.ExecuteScalarAsync<int>(sql, new { userId }) > 0;
        }
        public async Task<List<TeacherProfile>> GetBySchoolIdAsync(Guid schoolId)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"
        SELECT 
            tp.*,
            u.""Name"" AS UserName,
            u.""Email"",
            s.""Name"" AS SchoolName,
            d.""DepartmentName""
        FROM ""teacher_profile"" tp
        INNER JOIN ""user"" u ON tp.""UserId"" = u.""Id""
        LEFT JOIN ""school"" s ON u.""SchoolId"" = s.""Id""
        LEFT JOIN ""department"" d ON tp.""DepartmentId"" = d.""Id""
        WHERE u.""SchoolId"" = @schoolId
        ORDER BY u.""Name""";

            return (await conn.QueryAsync<TeacherProfile>(sql, new { schoolId })).AsList();
        }
        public async Task<List<TeacherProfile>> GetByDepartmentIdAsync(Guid departmentId)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"
        SELECT 
            tp.*,
            u.""Name"" AS UserName,
            u.""Email"",
            s.""Name"" AS SchoolName,
            d.""DepartmentName""
        FROM ""teacher_profile"" tp
        INNER JOIN ""user"" u ON tp.""UserId"" = u.""Id""
        LEFT JOIN ""school"" s ON u.""SchoolId"" = s.""Id""
        LEFT JOIN ""department"" d ON tp.""DepartmentId"" = d.""Id""
        WHERE tp.""DepartmentId"" = @departmentId
        ORDER BY u.""Name""";

            var result = await conn.QueryAsync<TeacherProfile>(sql, new { departmentId });
            return result.AsList();
        }
    }
}