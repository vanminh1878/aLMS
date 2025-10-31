// aLMS.Infrastructure.StudentAnswerInfra/StudentAnswerRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentAnswerEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Collections.Generic;
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
        public async Task AddRangeAsync(IEnumerable<StudentAnswer> answers)
        {
            await _context.Set<StudentAnswer>().AddRangeAsync(answers);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<StudentAnswer>> GetByStudentExerciseIdAsync(Guid studentExerciseId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT * FROM \"student_answer\" WHERE \"StudentExerciseId\" = @studentExerciseId";
            return await conn.QueryAsync<StudentAnswer>(sql, new { studentExerciseId });
        }
    }
}