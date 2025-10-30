// aLMS.Infrastructure.LessonInfra/LessonRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.LessonEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
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
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ""Id"", ""Title"", ""Description"", ""ResourceType"", ""Content"", 
                       ""IsRequired"", ""TopicId""
                FROM ""lesson""";
            return await conn.QueryAsync<Lesson>(sql);
        }

        public async Task<Lesson> GetLessonByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ""Id"", ""Title"", ""Description"", ""ResourceType"", ""Content"", 
                       ""IsRequired"", ""TopicId""
                FROM ""lesson""
                WHERE ""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<Lesson>(sql, new { id });
        }

        public async Task<IEnumerable<Lesson>> GetLessonsByTopicIdAsync(Guid topicId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ""Id"", ""Title"", ""Description"", ""ResourceType"", ""Content"", 
                       ""IsRequired"", ""TopicId""
                FROM ""lesson""
                WHERE ""TopicId"" = @topicId";
            return await conn.QueryAsync<Lesson>(sql, new { topicId });
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
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"lesson\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> LessonExistsAsync(Guid id)
        {
            return await _context.Set<Lesson>().AnyAsync(l => l.Id == id);
        }
    }
}