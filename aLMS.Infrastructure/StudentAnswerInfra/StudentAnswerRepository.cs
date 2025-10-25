using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentAnswerEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentAnswerInfra
{
    public class StudentAnswerRepository : IStudentAnswerRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public StudentAnswerRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<StudentAnswer>> GetAllStudentAnswersAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, StudentExerciseId, QuestionId, AnswerId, AnswerText, IsCorrect, SubmittedAt");
                sb.AppendLine("FROM \"StudentAnswer\"");
                return await connection.QueryAsync<StudentAnswer>(sb.ToString());
            }
        }

        public async Task<StudentAnswer> GetStudentAnswerByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, StudentExerciseId, QuestionId, AnswerId, AnswerText, IsCorrect, SubmittedAt");
                sb.AppendLine("FROM \"StudentAnswer\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<StudentAnswer>(sb.ToString(), new { Id = id });
            }
        }

        public async Task AddStudentAnswerAsync(StudentAnswer studentAnswer)
        {
            await _context.Set<StudentAnswer>().AddAsync(studentAnswer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStudentAnswerAsync(StudentAnswer studentAnswer)
        {
            _context.Set<StudentAnswer>().Update(studentAnswer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentAnswerAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"StudentAnswer\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<IEnumerable<StudentAnswer>> GetStudentAnswersByStudentExerciseIdAsync(Guid studentExerciseId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, StudentExerciseId, QuestionId, AnswerId, AnswerText, IsCorrect, SubmittedAt");
                sb.AppendLine("FROM \"StudentAnswer\"");
                sb.AppendLine("WHERE StudentExerciseId = @StudentExerciseId");
                return await connection.QueryAsync<StudentAnswer>(sb.ToString(), new { StudentExerciseId = studentExerciseId });
            }
        }

        public async Task<bool> StudentAnswerExistsAsync(Guid id)
        {
            return await _context.Set<StudentAnswer>().AnyAsync(sa => sa.Id == id);
        }
    }
}