using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentFinalTermRecordEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentFinalTermRecordInfra
{
    public class FinalTermRecordRepository : IFinalTermRecordRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public FinalTermRecordRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<FinalTermRecordDto?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        SELECT 
            r.""Id"",
            r.""StudentProfileId"",
            r.""ClassId"",
            r.""SubjectId"",                  
            r.""FinalScore"",
            r.""FinalEvaluation"",
            r.""Comment"",
            r.""CreatedAt"",
            r.""UpdatedAt"",
            sp.""UserId"",
            u.""Name"" AS StudentName,
            c.""ClassName"",
            sub.""Name"" AS SubjectName      
        FROM student_final_term_record r
        JOIN student_profile sp ON r.""StudentProfileId"" = sp.""UserId""
        JOIN ""user"" u ON sp.""UserId"" = u.""Id""
        LEFT JOIN ""class"" c ON r.""ClassId"" = c.""Id""
        JOIN ""subject"" sub ON r.""SubjectId"" = sub.""Id"" 
        WHERE r.""Id"" = @id";


            return await conn.QuerySingleOrDefaultAsync<FinalTermRecordDto>(sql, new { id });
        }

        public async Task<List<FinalTermRecordDto>> GetByStudentIdAsync(Guid studentProfileId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        SELECT 
            r.""Id"",
            r.""StudentProfileId"",
            r.""ClassId"",
            r.""SubjectId"",
            r.""FinalScore"",
            r.""FinalEvaluation"",
            r.""Comment"",
            r.""CreatedAt"",
            r.""UpdatedAt"",
            sp.""UserId"",
            u.""Name"" AS StudentName,
            c.""ClassName"",
            sub.""Name"" AS SubjectName
        FROM student_final_term_record r
        JOIN student_profile sp ON r.""StudentProfileId"" = sp.""UserId""
        JOIN ""user"" u ON sp.""UserId"" = u.""Id""
        LEFT JOIN ""class"" c ON r.""ClassId"" = c.""Id""
        JOIN ""subject"" sub ON r.""SubjectId"" = sub.""Id""
        WHERE r.""StudentProfileId"" = @studentProfileId
        ORDER BY sub.""Name"", r.""CreatedAt"" DESC";

            var result = await conn.QueryAsync<FinalTermRecordDto>(sql, new { studentProfileId });
            return result.ToList();
        }

        public async Task<List<FinalTermRecordDto>> GetByClassIdAsync(Guid classId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        SELECT 
            r.""Id"",
            r.""StudentProfileId"",
            r.""ClassId"",
            r.""SubjectId"",
            r.""FinalScore"",
            r.""FinalEvaluation"",
            r.""Comment"",
            r.""CreatedAt"",
            r.""UpdatedAt"",
            sp.""UserId"",
            u.""Name"" AS StudentName,
            c.""ClassName"",
            sub.""Name"" AS SubjectName
        FROM student_final_term_record r
        JOIN student_profile sp ON r.""StudentProfileId"" = sp.""UserId""
        JOIN ""user"" u ON sp.""UserId"" = u.""Id""
        LEFT JOIN ""class"" c ON r.""ClassId"" = c.""Id""
        JOIN ""subject"" sub ON r.""SubjectId"" = sub.""Id""
        WHERE r.""ClassId"" = @classId
        ORDER BY u.""Name"", sub.""Name""";

            var result = await conn.QueryAsync<FinalTermRecordDto>(sql, new { classId });
            return result.ToList();
        }

        public async Task AddAsync(StudentFinalTermRecord record)
        {
            await _context.Set<StudentFinalTermRecord>().AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentFinalTermRecord record)
        {
            _context.Set<StudentFinalTermRecord>().Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(
                @"DELETE FROM student_final_term_record WHERE ""Id"" = @id",
                new { id });
        }
        public async Task<StudentFinalTermRecord?> GetEntityByIdAsync(Guid id)
        {
            return await _context.Set<StudentFinalTermRecord>()
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<bool> ExistsForStudentAsync(Guid studentProfileId, Guid? classId = null)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"SELECT COUNT(1) FROM student_final_term_record 
                       WHERE ""StudentProfileId"" = @studentProfileId";

            var parameters = new DynamicParameters();
            parameters.Add("@studentProfileId", studentProfileId);

            if (classId.HasValue)
            {
                sql += " AND \"ClassId\" = @classId";
                parameters.Add("@classId", classId.Value);
            }

            var count = await conn.ExecuteScalarAsync<int>(sql, parameters);
            return count > 0;
        }
    }
}