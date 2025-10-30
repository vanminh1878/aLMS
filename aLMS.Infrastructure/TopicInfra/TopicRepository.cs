using aLMS.Application.Common.Interfaces;
using aLMS.Domain.TopicEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.TopicInfra
{
    public class TopicRepository : ITopicRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public TopicRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"Title\", \"DateFrom\", \"DateTo\", \"SubjectId\" FROM \"topic\"";
            return await conn.QueryAsync<Topic>(sql);
        }

        public async Task<Topic> GetTopicByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"Title\", \"DateFrom\", \"DateTo\", \"SubjectId\" FROM \"topic\" WHERE \"Id\" = @id";
            return await conn.QuerySingleOrDefaultAsync<Topic>(sql, new { id });
        }

        public async Task AddTopicAsync(Topic topic)
        {
            await _context.Set<Topic>().AddAsync(topic);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTopicAsync(Topic topic)
        {
            _context.Set<Topic>().Update(topic);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTopicAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"topic\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> TopicExistsAsync(Guid id)
        {
            return await _context.Set<Topic>().AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Topic>> GetTopicsBySubjectIdAsync(Guid subjectId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"Title\", \"DateFrom\", \"DateTo\", \"SubjectId\" FROM \"topic\" WHERE \"SubjectId\" = @subjectId";
            return await conn.QueryAsync<Topic>(sql, new { subjectId });
        }
    }
}