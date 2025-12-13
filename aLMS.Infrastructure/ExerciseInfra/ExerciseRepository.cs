// aLMS.Infrastructure.ExerciseInfra/ExerciseRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ExerciseEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.ExerciseInfra
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public ExerciseRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ""Id"", ""Title"", ""ExerciseFile"", ""HasTimeLimit"", ""TimeLimit"", 
                       ""QuestionLayout"", ""OrderNumber"", ""TotalScore""
                FROM ""exercise""";
            return await conn.QueryAsync<Exercise>(sql);
        }

        public async Task<Exercise> GetExerciseByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ""Id"", ""Title"", ""ExerciseFile"", ""HasTimeLimit"", ""TimeLimit"", 
                       ""QuestionLayout"", ""OrderNumber"", ""TotalScore""
                FROM ""exercise""
                WHERE ""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<Exercise>(sql, new { id });
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByTopicIdAsync(Guid topicId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ""Id"", ""Title"", ""ExerciseFile"", ""HasTimeLimit"", ""TimeLimit"", 
                       ""QuestionLayout"", ""OrderNumber"", ""TotalScore""
                FROM ""exercise""
                WHERE ""TopicId"" = @topicId";
            return await conn.QueryAsync<Exercise>(sql, new { topicId });
        }

        public async Task AddExerciseAsync(Exercise exercise)
        {
            await _context.Set<Exercise>().AddAsync(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExerciseAsync(Exercise exercise)
        {
            _context.Set<Exercise>().Update(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExerciseAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"exercise\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> ExerciseExistsAsync(Guid id)
        {
            return await _context.Set<Exercise>().AnyAsync(e => e.Id == id);
        }
    }
}