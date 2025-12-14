// aLMS.Infrastructure.ParentProfileInfra/ParentProfileRepository.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ParentProfileEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
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

        public async Task<ParentProfile?> GetByParentIdAsync(Guid parentId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT pp.*, 
                       p.""Name"" as ParentName, p.""Email"" as ParentEmail,
                       s.""Name"" as StudentName, s.""Email"" as StudentEmail
                FROM ""parent_profile"" pp
                JOIN ""user"" p ON pp.""UserId"" = p.""Id""
                JOIN ""user"" s ON pp.""StudentId"" = s.""Id""
                WHERE pp.""UserId"" = @parentId";
            return await conn.QuerySingleOrDefaultAsync<ParentProfile>(sql, new { parentId });
        }

        public async Task<IEnumerable<ParentProfileDto>> GetByStudentIdAsync(Guid studentId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        SELECT 
            pp.""UserId"" AS ParentId, 
            pp.""StudentId"",
            pu.""Name"" AS ParentName,
            pu.""Email"" AS ParentEmail,
            pu.""PhoneNumber"" AS ParentPhone,
            pu.""DateOfBirth"" AS ParentDateOfBirth,
            pu.""Gender"" AS ParentGender,
            pu.""Address"" AS ParentAddress,
            
            su.""Name"" AS StudentName,
            su.""Email"" AS StudentEmail
        FROM ""parent_profile"" pp
        JOIN ""user"" pu ON pp.""UserId"" = pu.""Id""        -- pu: parent user
        JOIN ""user"" su ON pp.""StudentId"" = su.""Id""     -- su: student user
        WHERE pp.""StudentId"" = @studentId";

            var result = await conn.QueryAsync<ParentProfileDto>(sql, new { studentId });
            return result;
        }

        public async Task AddAsync(ParentProfile profile)
        {
            await _context.Set<ParentProfile>().AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ParentProfile profile)
        {
            _context.Set<ParentProfile>().Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid parentId, Guid studentId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"parent_profile\" WHERE \"UserId\" = @parentId AND \"StudentId\" = @studentId";
            await conn.ExecuteAsync(sql, new { parentId, studentId });
        }

        public async Task<bool> ExistsAsync(Guid parentId, Guid studentId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM \"parent_profile\" WHERE \"UserId\" = @parentId AND \"StudentId\" = @studentId";
            return await conn.ExecuteScalarAsync<int>(sql, new { parentId, studentId }) > 0;
        }
    }
}