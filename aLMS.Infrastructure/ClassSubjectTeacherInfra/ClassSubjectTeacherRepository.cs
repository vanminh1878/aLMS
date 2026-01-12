using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassSubjectEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.ClassSubjectTeacherInfra
{
    public class ClassSubjectTeacherRepository : IClassSubjectTeacherRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public ClassSubjectTeacherRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task AddAsync(ClassSubjectTeacher assignment)
        {
            await _context.Set<ClassSubjectTeacher>().AddAsync(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<ClassSubjectTeacher?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT cst.""Id"", cst.""ClassSubjectId"", cst.""TeacherId"", cst.""SchoolYear"", cst.""CreatedAt""
                FROM ""class_subject_teacher"" cst
                WHERE cst.""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<ClassSubjectTeacher>(sql, new { id });
        }

        public async Task<ClassSubjectTeacher?> GetByClassSubjectAndTeacherAsync(Guid classSubjectId, Guid teacherId, string? schoolYear)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT cst.""Id"", cst.""ClassSubjectId"", cst.""TeacherId"", cst.""SchoolYear"", cst.""CreatedAt""
                FROM ""class_subject_teacher"" cst
                WHERE cst.""ClassSubjectId"" = @classSubjectId 
                  AND cst.""TeacherId"" = @teacherId 
                  AND (@schoolYear IS NULL OR cst.""SchoolYear"" = @schoolYear)";
            return await conn.QuerySingleOrDefaultAsync<ClassSubjectTeacher>(sql, new { classSubjectId, teacherId, schoolYear });
        }

        public async Task<IEnumerable<ClassSubjectTeacher>> GetTeachersByClassSubjectAsync(Guid classSubjectId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT cst.""Id"", cst.""ClassSubjectId"", cst.""TeacherId"", cst.""SchoolYear"", cst.""CreatedAt"",
                       tp.""Name"" AS ""TeacherName""
                FROM ""class_subject_teacher"" cst
                INNER JOIN ""teacher_profile"" tp ON cst.""TeacherId"" = tp.""UserId""
                WHERE cst.""ClassSubjectId"" = @classSubjectId";
            return await conn.QueryAsync<ClassSubjectTeacher>(sql, new { classSubjectId });
        }

        public async Task<IEnumerable<ClassSubject>> GetClassSubjectsByTeacherAsync(Guid teacherId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT cs.""Id"", cs.""ClassId"", cs.""SubjectId"", cs.""SchoolYear"", cs.""CreatedAt"",
                       c.""ClassName"", s.""Name"" AS ""SubjectName""
                FROM ""class_subject_teacher"" cst
                INNER JOIN ""class_subject"" cs ON cst.""ClassSubjectId"" = cs.""Id""
                INNER JOIN ""class"" c ON cs.""ClassId"" = c.""Id""
                INNER JOIN ""subject"" s ON cs.""SubjectId"" = s.""Id""
                WHERE cst.""TeacherId"" = @teacherId";
            return await conn.QueryAsync<ClassSubject>(sql, new { teacherId });
        }
    }
}