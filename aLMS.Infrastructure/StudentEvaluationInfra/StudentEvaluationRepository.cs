using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentEvaluationEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentEvaluationInfra
{
    public class StudentEvaluationRepository : IStudentEvaluationRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public StudentEvaluationRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task AddAsync(StudentEvaluation entity)
        {
            await _context.Set<StudentEvaluation>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentEvaluation entity)
        {
            _context.Set<StudentEvaluation>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<StudentEvaluation?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT se.*, 
                       u.""Name"" AS ""StudentName"", 
                       c.""ClassName"", 
                       cu.""Name"" AS ""CreatedByName""
                FROM ""student_evaluation"" se
                INNER JOIN ""user"" u ON se.""StudentId"" = u.""Id""
                INNER JOIN ""class"" c ON se.""ClassId"" = c.""Id""
                INNER JOIN ""user"" cu ON se.""CreatedBy"" = cu.""Id""
                WHERE se.""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<StudentEvaluation>(sql, new { id });
        }

        public async Task<IEnumerable<StudentEvaluation>> GetByStudentIdAsync(Guid studentId, string? semester, string? schoolYear)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT se.*, 
                       u.""Name"" AS ""StudentName"", 
                       c.""ClassName"", 
                       cu.""Name"" AS ""CreatedByName""
                FROM ""student_evaluation"" se
                INNER JOIN ""user"" u ON se.""StudentId"" = u.""Id""
                INNER JOIN ""class"" c ON se.""ClassId"" = c.""Id""
                INNER JOIN ""user"" cu ON se.""CreatedBy"" = cu.""Id""
                WHERE se.""StudentId"" = @studentId";

            if (!string.IsNullOrEmpty(semester))
            {
                sql += " AND se.\"Semester\" = @semester";
            }

            if (!string.IsNullOrEmpty(schoolYear))
            {
                sql += " AND se.\"SchoolYear\" = @schoolYear";
            }

            sql += " ORDER BY se.\"CreatedAt\" DESC";

            return await conn.QueryAsync<StudentEvaluation>(sql, new { studentId, semester, schoolYear });
        }

        public async Task<IEnumerable<StudentEvaluation>> GetByClassIdAsync(Guid classId, string? semester, string? schoolYear)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT se.*, 
                       u.""Name"" AS ""StudentName"", 
                       c.""ClassName"", 
                       cu.""Name"" AS ""CreatedByName""
                FROM ""student_evaluation"" se
                INNER JOIN ""user"" u ON se.""StudentId"" = u.""Id""
                INNER JOIN ""class"" c ON se.""ClassId"" = c.""Id""
                INNER JOIN ""user"" cu ON se.""CreatedBy"" = cu.""Id""
                WHERE se.""ClassId"" = @classId";

            if (!string.IsNullOrEmpty(semester))
            {
                sql += " AND se.\"Semester\" = @semester";
            }

            if (!string.IsNullOrEmpty(schoolYear))
            {
                sql += " AND se.\"SchoolYear\" = @schoolYear";
            }

            sql += " ORDER BY se.\"CreatedAt\" DESC";

            return await conn.QueryAsync<StudentEvaluation>(sql, new { classId, semester, schoolYear });
        }
    }
}