using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassSubjectEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.ClassSubjectInfra
{
    public class ClassSubjectRepository : IClassSubjectRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public ClassSubjectRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task AddClassSubjectAsync(ClassSubject classSubject)
        {
            await _context.Set<ClassSubject>().AddAsync(classSubject);
            await _context.SaveChangesAsync();
        }
        public async Task<ClassSubject?> GetClassSubjectByClassAndSubjectAsync(Guid classId, Guid subjectId, string? schoolYear)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        SELECT cs.""Id"", cs.""ClassId"", cs.""SubjectId"", cs.""SchoolYear"", cs.""CreatedAt""
        FROM ""class_subject"" cs
        WHERE cs.""ClassId"" = @classId 
          AND cs.""SubjectId"" = @subjectId 
          AND (@schoolYear IS NULL OR cs.""SchoolYear"" = @schoolYear)";

            return await conn.QuerySingleOrDefaultAsync<ClassSubject>(sql, new { classId, subjectId, schoolYear });
        }
        public async Task<ClassSubject?> GetClassSubjectByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT cs.""Id"", cs.""ClassId"", cs.""SubjectId"", cs.""SchoolYear"", cs.""CreatedAt"",
                       c.""ClassName"", s.""Name"" AS ""SubjectName""
                FROM ""class_subject"" cs
                INNER JOIN ""class"" c ON cs.""ClassId"" = c.""Id""
                INNER JOIN ""subject"" s ON cs.""SubjectId"" = s.""Id""
                WHERE cs.""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<ClassSubject>(sql, new { id });
        }

        public async Task<IEnumerable<ClassSubject>> GetAllClassSubjectsAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT cs.""Id"", cs.""ClassId"", cs.""SubjectId"", cs.""SchoolYear"", cs.""CreatedAt"",
                       c.""ClassName"", s.""Name"" AS ""SubjectName""
                FROM ""class_subject"" cs
                INNER JOIN ""class"" c ON cs.""ClassId"" = c.""Id""
                INNER JOIN ""subject"" s ON cs.""SubjectId"" = s.""Id""";
            return await conn.QueryAsync<ClassSubject>(sql);
        }

        public async Task<IEnumerable<ClassSubjectDto>> GetClassSubjectsByClassIdAsync(Guid classId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT cs.""Id"", cs.""ClassId"", cs.""SubjectId"", cs.""SchoolYear"", cs.""CreatedAt"",
                       c.""ClassName"", s.""Name"" AS ""SubjectName""
                FROM ""class_subject"" cs
                INNER JOIN ""class"" c ON cs.""ClassId"" = c.""Id""
                INNER JOIN ""subject"" s ON cs.""SubjectId"" = s.""Id""
                WHERE cs.""ClassId"" = @classId";
            return await conn.QueryAsync<ClassSubjectDto>(sql, new { classId });
        }

        public async Task<IEnumerable<ClassSubject>> GetClassSubjectsBySubjectIdAsync(Guid subjectId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT cs.""Id"", cs.""ClassId"", cs.""SubjectId"", cs.""SchoolYear"", cs.""CreatedAt"",
                       c.""ClassName"", s.""Name"" AS ""SubjectName""
                FROM ""class_subject"" cs
                INNER JOIN ""class"" c ON cs.""ClassId"" = c.""Id""
                INNER JOIN ""subject"" s ON cs.""SubjectId"" = s.""Id""
                WHERE cs.""SubjectId"" = @subjectId";
            return await conn.QueryAsync<ClassSubject>(sql, new { subjectId });
        }

        public async Task UpdateClassSubjectAsync(ClassSubject classSubject)
        {
            _context.Set<ClassSubject>().Update(classSubject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClassSubjectAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"class_subject\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }
    }
}