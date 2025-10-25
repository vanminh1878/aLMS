using aLMS.Application.Common.Interfaces;
using aLMS.Domain.LessonEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.LessonInfra
{
    public class LessonRepository : ILessonRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public LessonRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Title, Description, ResourceType, Content, IsRequired, TopicId");
                sb.AppendLine("FROM \"Lesson\"");
                return await connection.QueryAsync<Lesson>(sb.ToString());
            }
        }

        public async Task<Lesson> GetLessonByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Title, Description, ResourceType, Content, IsRequired, TopicId");
                sb.AppendLine("FROM \"Lesson\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Lesson>(sb.ToString(), new { Id = id });
            }
        }

        public async Task AddLessonAsync(Lesson lesson)
        {
            await _context.Set<Lesson>().AddAsync(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLessonAsync(Lesson lesson)
        {
            _context.Set<Lesson>().Update(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLessonAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Lesson\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<IEnumerable<Lesson>> GetLessonsByTopicIdAsync(Guid topicId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Title, Description, ResourceType, Content, IsRequired, TopicId");
                sb.AppendLine("FROM \"Lesson\"");
                sb.AppendLine("WHERE TopicId = @TopicId");
                return await connection.QueryAsync<Lesson>(sb.ToString(), new { TopicId = topicId });
            }
        }

        public async Task<bool> LessonExistsAsync(Guid id)
        {
            return await _context.Set<Lesson>().AnyAsync(l => l.Id == id);
        }
    }
}