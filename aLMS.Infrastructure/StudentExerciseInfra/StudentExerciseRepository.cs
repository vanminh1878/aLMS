// aLMS.Infrastructure.StudentExerciseInfra/StudentExerciseRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentExerciseEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
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

        public async Task<StudentExercise?> GetActiveByStudentAndExerciseAsync(Guid studentId, Guid exerciseId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT * FROM ""student_exercise"" 
                WHERE ""StudentId"" = @studentId 
                  AND ""ExerciseId"" = @exerciseId 
                  AND ""IsCompleted"" = false
                ORDER BY ""StartTime"" DESC LIMIT 1";
            return await conn.QuerySingleOrDefaultAsync<StudentExercise>(sql, new { studentId, exerciseId });
        }

        public async Task<StudentExercise?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT * FROM \"student_exercise\" WHERE \"Id\" = @id";
            return await conn.QuerySingleOrDefaultAsync<StudentExercise>(sql, new { id });
        }

        public async Task AddAsync(StudentExercise se)
        {
            await _context.Set<StudentExercise>().AddAsync(se);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentExercise se)
        {
            _context.Set<StudentExercise>().Update(se);
            await _context.SaveChangesAsync();
        }
        public async Task<List<StudentExercise>> GetByExerciseAndStudentIdsAsync(Guid exerciseId, List<Guid> studentIds)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        SELECT * FROM ""student_exercise"" 
        WHERE ""ExerciseId"" = @exerciseId 
          AND ""StudentId"" = ANY(@studentIds)
        ORDER BY ""StartTime"" DESC";

            return (await conn.QueryAsync<StudentExercise>(sql, new { exerciseId, studentIds })).ToList();
        }
    }
}