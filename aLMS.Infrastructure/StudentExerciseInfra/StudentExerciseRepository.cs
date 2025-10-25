using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentExerciseEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentExerciseInfra
{
    public class StudentExerciseRepository : IStudentExerciseRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public StudentExerciseRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<StudentExercise>> GetAllStudentExercisesAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, StudentId, ExerciseId, StartTime, EndTime, Score, IsCompleted, AttemptNumber");
                sb.AppendLine("FROM \"StudentExercise\"");
                return await connection.QueryAsync<StudentExercise>(sb.ToString());
            }
        }

        public async Task<StudentExercise> GetStudentExerciseByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, StudentId, ExerciseId, StartTime, EndTime, Score, IsCompleted, AttemptNumber");
                sb.AppendLine("FROM \"StudentExercise\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<StudentExercise>(sb.ToString(), new { Id = id });
            }
        }

        public async Task AddStudentExerciseAsync(StudentExercise studentExercise)
        {
            await _context.Set<StudentExercise>().AddAsync(studentExercise);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStudentExerciseAsync(StudentExercise studentExercise)
        {
            _context.Set<StudentExercise>().Update(studentExercise);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentExerciseAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"StudentExercise\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<IEnumerable<StudentExercise>> GetStudentExercisesByStudentIdAsync(Guid studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, StudentId, ExerciseId, StartTime, EndTime, Score, IsCompleted, AttemptNumber");
                sb.AppendLine("FROM \"StudentExercise\"");
                sb.AppendLine("WHERE StudentId = @StudentId");
                return await connection.QueryAsync<StudentExercise>(sb.ToString(), new { StudentId = studentId });
            }
        }

        public async Task<bool> StudentExerciseExistsAsync(Guid id)
        {
            return await _context.Set<StudentExercise>().AnyAsync(se => se.Id == id);
        }
    }
}