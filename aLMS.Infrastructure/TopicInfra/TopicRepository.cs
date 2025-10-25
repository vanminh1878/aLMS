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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Title, DateFrom, DateTo, SubjectId");
                sb.AppendLine("FROM \"Topic\"");
                return await connection.QueryAsync<Topic>(sb.ToString());
            }
        }

        public async Task<Topic> GetTopicByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Title, DateFrom, DateTo, SubjectId");
                sb.AppendLine("FROM \"Topic\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Topic>(sb.ToString(), new { Id = id });
            }
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Topic\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<IEnumerable<Topic>> GetTopicsBySubjectIdAsync(Guid subjectId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Title, DateFrom, DateTo, SubjectId");
                sb.AppendLine("FROM \"Topic\"");
                sb.AppendLine("WHERE SubjectId = @SubjectId");
                return await connection.QueryAsync<Topic>(sb.ToString(), new { SubjectId = subjectId });
            }
        }

        public async Task<bool> TopicExistsAsync(Guid id)
        {
            return await _context.Set<Topic>().AnyAsync(t => t.Id == id);
        }
    }
}