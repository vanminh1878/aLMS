using aLMS.Application.Common.Interfaces;
using aLMS.Domain.QuestionEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, ExerciseId, QuestionContent, QuestionImage, QuestionType, OrderNumber, Score, Explanation");
                sb.AppendLine("FROM \"Question\"");
                return await connection.QueryAsync<Question>(sb.ToString());
            }
        }

        public async Task<Question> GetQuestionByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, ExerciseId, QuestionContent, QuestionImage, QuestionType, OrderNumber, Score, Explanation");
                sb.AppendLine("FROM \"Question\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Question>(sb.ToString(), new { Id = id });
            }
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Question\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExerciseIdAsync(Guid exerciseId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, ExerciseId, QuestionContent, QuestionImage, QuestionType, OrderNumber, Score, Explanation");
                sb.AppendLine("FROM \"Question\"");
                sb.AppendLine("WHERE ExerciseId = @ExerciseId");
                return await connection.QueryAsync<Question>(sb.ToString(), new { ExerciseId = exerciseId });
            }
        }

        public async Task<bool> QuestionExistsAsync(Guid id)
        {
            return await _context.Set<Question>().AnyAsync(q => q.Id == id);
        }
    }
}