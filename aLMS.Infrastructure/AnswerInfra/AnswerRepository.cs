using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AnswerEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.AnswerInfra
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public AnswerRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Answer>> GetAllAnswersAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, QuestionId, AnswerContent, IsCorrect, OrderNumber");
                sb.AppendLine("FROM \"Answer\"");
                return await connection.QueryAsync<Answer>(sb.ToString());
            }
        }

        public async Task<Answer> GetAnswerByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, QuestionId, AnswerContent, IsCorrect, OrderNumber");
                sb.AppendLine("FROM \"Answer\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Answer>(sb.ToString(), new { Id = id });
            }
        }

        public async Task AddAnswerAsync(Answer answer)
        {
            await _context.Set<Answer>().AddAsync(answer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAnswerAsync(Answer answer)
        {
            _context.Set<Answer>().Update(answer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnswerAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Answer\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, QuestionId, AnswerContent, IsCorrect, OrderNumber");
                sb.AppendLine("FROM \"Answer\"");
                sb.AppendLine("WHERE QuestionId = @QuestionId");
                return await connection.QueryAsync<Answer>(sb.ToString(), new { QuestionId = questionId });
            }
        }

        public async Task<bool> AnswerExistsAsync(Guid id)
        {
            return await _context.Set<Answer>().AnyAsync(a => a.Id == id);
        }
    }
}