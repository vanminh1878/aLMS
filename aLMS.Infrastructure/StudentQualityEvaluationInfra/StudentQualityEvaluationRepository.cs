using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentQualityEvaluationEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentQualityEvaluationInfra
{
    public class StudentQualityEvaluationRepository : IStudentQualityEvaluationRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public StudentQualityEvaluationRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task AddAsync(StudentQualityEvaluation entity)
        {
            await _context.Set<StudentQualityEvaluation>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<StudentQualityEvaluation?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT sqe.*, q.""Name"" AS ""QualityName""
                FROM ""student_quality_evaluation"" sqe
                INNER JOIN ""quality"" q ON sqe.""QualityId"" = q.""Id""
                WHERE sqe.""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<StudentQualityEvaluation>(sql, new { id });
        }
    }
}