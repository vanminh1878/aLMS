using aLMS.Application.Common.DTOs;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.SubjectEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.SubjectInfra
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public SubjectRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"Name\", \"Description\", \"Category\" FROM \"subject\""; 
            return await conn.QueryAsync<Subject>(sql);
        }

        public async Task<Subject> GetSubjectByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"Name\", \"Description\", \"Category\" FROM \"subject\" WHERE \"Id\" = @id"; 
            return await conn.QuerySingleOrDefaultAsync<Subject>(sql, new { id });
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            await _context.Set<Subject>().AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            _context.Set<Subject>().Update(subject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubjectAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"subject\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> SubjectExistsAsync(Guid id)
        {
            return await _context.Set<Subject>().AnyAsync(s => s.Id == id);
        }

        //public async Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(Guid classId)
        //{
        //    using var conn = new NpgsqlConnection(_connectionString);
        //    var sql = "SELECT \"Id\", \"Name\", \"Description\", \"Category\", \"ClassId\" FROM \"subject\" WHERE \"ClassId\" = @classId";
        //    return await conn.QueryAsync<Subject>(sql, new { classId });
        //}
        public async Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(Guid classId)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"
                        SELECT s.""Id"",
                               s.""Name"",
                               s.""Description"",
                               s.""Category""
                        FROM ""subject"" s
                        INNER JOIN ""class_subject"" cs 
                            ON s.""Id"" = cs.""SubjectId""
                        WHERE cs.""ClassId"" = @classId
                    ";

            return await conn.QueryAsync<Subject>(sql, new { classId });
        }
        public async Task<Subject?> GetSubjectByTopicIdAsync(Guid topicId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        SELECT s.""Id"", s.""Name"", s.""Description"", s.""Category"", s.""ClassId"", c.""ClassName""
        FROM ""subject"" s
        INNER JOIN ""topic"" t ON s.""Id"" = t.""SubjectId""
        LEFT JOIN ""class"" c ON s.""ClassId"" = c.""Id""
        WHERE t.""Id"" = @topicId";

            return await conn.QuerySingleOrDefaultAsync<Subject>(sql, new { topicId });
        }
        public async Task<List<AssignedSubjectDto>> GetAssignedSubjectsByTeacherAsync(
            Guid teacherId,
            string? schoolYear = null)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    s.""Id""            AS SubjectId,
                    s.""Name""          AS SubjectName,
                    s.""Description"",
                    s.""Category"",
                    cs.""ClassId"",
                    c.""ClassName"",
                    cst.""SchoolYear""
                FROM ""subject"" s
                INNER JOIN ""class_subject"" cs 
                    ON s.""Id"" = cs.""SubjectId""
                INNER JOIN ""class_subject_teacher"" cst
                    ON cs.""Id"" = cst.""ClassSubjectId""
                LEFT JOIN ""class"" c
                    ON cs.""ClassId"" = c.""Id""
                WHERE cst.""TeacherId"" = @TeacherId
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@TeacherId", teacherId);

            if (!string.IsNullOrWhiteSpace(schoolYear))
            {
                sql += " AND cst.\"SchoolYear\" = @SchoolYear";
                parameters.Add("@SchoolYear", schoolYear);
            }

            sql += " ORDER BY cst.\"SchoolYear\" DESC, s.\"Name\"";

            var result = await conn.QueryAsync<AssignedSubjectDto>(sql, parameters);

            return result.AsList();
        }


    }
}