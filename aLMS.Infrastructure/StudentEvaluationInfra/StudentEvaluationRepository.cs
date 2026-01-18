using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentEvaluationEntity;
using aLMS.Domain.StudentQualityEvaluationEntity;
using aLMS.Domain.StudentSubjectCommentEntity;
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

        public async Task<IEnumerable<StudentEvaluation>> GetByStudentIdAsync(
      Guid studentId,
      string? semester,
      string? schoolYear)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            // 1. Lấy danh sách các evaluation chính
            var sqlMain = @"
        SELECT se.*, 
               u.""Name"" AS ""StudentName"", 
               c.""ClassName"", 
               cu.""Name"" AS ""CreatedByName""
        FROM ""student_evaluation"" se
        INNER JOIN ""user"" u ON se.""StudentId"" = u.""Id""
        INNER JOIN ""class"" c ON se.""ClassId"" = c.""Id""
        INNER JOIN ""user"" cu ON se.""CreatedBy"" = cu.""Id""
        WHERE se.""StudentId"" = @studentId
        /* AND điều kiện semester, schoolYear nếu cần */
        ORDER BY se.""CreatedAt"" DESC";

            var evaluations = (await conn.QueryAsync<StudentEvaluation>(sqlMain, new { studentId, semester, schoolYear }))
                .ToList();

            if (!evaluations.Any()) return evaluations;

            var evaluationIds = evaluations.Select(e => e.Id).ToList();

            // 2. Lấy tất cả subject comments của các evaluation trên
            var sqlComments = @"
        SELECT * 
        FROM student_subject_comment ssc
        WHERE ssc.""StudentEvaluationId"" = ANY(@Ids)
        "; 

            var allComments = await conn.QueryAsync<StudentSubjectComment>(
                sqlComments,
                new { Ids = evaluationIds }
            );

            // 3. Lấy tất cả quality evaluations
            var sqlQualities = @"
        SELECT * 
        FROM student_quality_evaluation sqe
        WHERE sqe.""StudentEvaluationId"" = ANY(@Ids)
        ";  

            var allQualities = await conn.QueryAsync<StudentQualityEvaluation>(
                sqlQualities,
                new { Ids = evaluationIds }
            );

            // 4. Group và gán vào từng evaluation
            var commentsByEval = allComments.GroupBy(c => c.StudentEvaluationId).ToDictionary(
                g => g.Key,
                g => g.ToList()
            );

            var qualitiesByEval = allQualities.GroupBy(q => q.StudentEvaluationId).ToDictionary(
                g => g.Key,
                g => g.ToList()
            );

            foreach (var eval in evaluations)
            {
                eval.SubjectComments = commentsByEval.TryGetValue(eval.Id, out var comments)
                    ? comments
                    : new List<StudentSubjectComment>();

                eval.QualityEvaluations = qualitiesByEval.TryGetValue(eval.Id, out var qualities)
                    ? qualities
                    : new List<StudentQualityEvaluation>();
            }

            return evaluations;
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