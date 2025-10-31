// aLMS.Infrastructure.AnswerInfra/AnswerRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AnswerEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(Guid questionId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ""Id"", ""AnswerContent"", ""IsCorrect"", ""OrderNumber"", ""QuestionId""
                FROM ""answer"" WHERE ""QuestionId"" = @questionId";
            return await conn.QueryAsync<Answer>(sql, new { questionId });
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
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"answer\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> AnswerExistsAsync(Guid id)
        {
            return await _context.Set<Answer>().AnyAsync(a => a.Id == id);
        }
    }
}