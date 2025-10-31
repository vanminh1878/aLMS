// aLMS.Infrastructure.QuestionInfra/QuestionRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.QuestionEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.QuestionInfra
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public QuestionRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ""Id"", ""QuestionContent"", ""QuestionImage"", ""QuestionType"", 
                       ""OrderNumber"", ""Score"", ""Explanation"", ""ExerciseId""
                FROM ""question""";
            return await conn.QueryAsync<Question>(sql);
        }

        public async Task<Question> GetQuestionByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ""Id"", ""QuestionContent"", ""QuestionImage"", ""QuestionType"", 
                       ""OrderNumber"", ""Score"", ""Explanation"", ""ExerciseId""
                FROM ""question"" WHERE ""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<Question>(sql, new { id });
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExerciseIdAsync(Guid exerciseId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ""Id"", ""QuestionContent"", ""QuestionImage"", ""QuestionType"", 
                       ""OrderNumber"", ""Score"", ""Explanation"", ""ExerciseId""
                FROM ""question"" WHERE ""ExerciseId"" = @exerciseId";
            return await conn.QueryAsync<Question>(sql, new { exerciseId });
        }

        public async Task AddQuestionAsync(Question question)
        {
            await _context.Set<Question>().AddAsync(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            _context.Set<Question>().Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"question\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> QuestionExistsAsync(Guid id)
        {
            return await _context.Set<Question>().AnyAsync(q => q.Id == id);
        }
    }
}