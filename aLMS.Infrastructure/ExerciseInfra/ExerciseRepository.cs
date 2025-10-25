using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ExerciseEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Title, ExerciseFile, HasTimeLimit, TimeLimit, QuestionLayout, OrderNumber, TotalScore, LessonId");
                sb.AppendLine("FROM \"Exercise\"");
                return await connection.QueryAsync<Exercise>(sb.ToString());
            }
        }

        public async Task<Exercise> GetExerciseByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Title, ExerciseFile, HasTimeLimit, TimeLimit, QuestionLayout, OrderNumber, TotalScore, LessonId");
                sb.AppendLine("FROM \"Exercise\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Exercise>(sb.ToString(), new { Id = id });
            }
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Exercise\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByLessonIdAsync(Guid lessonId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Title, ExerciseFile, HasTimeLimit, TimeLimit, QuestionLayout, OrderNumber, TotalScore, LessonId");
                sb.AppendLine("FROM \"Exercise\"");
                sb.AppendLine("WHERE LessonId = @LessonId");
                return await connection.QueryAsync<Exercise>(sb.ToString(), new { LessonId = lessonId });
            }
        }

        public async Task<bool> ExerciseExistsAsync(Guid id)
        {
            return await _context.Set<Exercise>().AnyAsync(e => e.Id == id);
        }
    }
}