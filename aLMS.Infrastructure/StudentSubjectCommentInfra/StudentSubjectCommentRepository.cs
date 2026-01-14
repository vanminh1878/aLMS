using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentEvaluationEntity;
using aLMS.Domain.StudentSubjectCommentEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentSubjectCommentInfra
{
    public class StudentSubjectCommentRepository : IStudentSubjectCommentRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public StudentSubjectCommentRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task AddAsync(StudentSubjectComment entity)
        {
            await _context.Set<StudentSubjectComment>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<StudentSubjectComment?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT ssc.*, 
                       s.""Name"" AS ""SubjectName""
                FROM ""student_subject_comment"" ssc
                INNER JOIN ""subject"" s ON ssc.""SubjectId"" = s.""Id""
                WHERE ssc.""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<StudentSubjectComment>(sql, new { id });
        }
    }
}