using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.TimetableEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.TimetableInfra
{
    public class TimetableRepository : ITimetableRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public TimetableRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task AddAsync(Timetable timetable)
        {
            await _context.Set<Timetable>().AddAsync(timetable);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Timetable timetable)
        {
            _context.Set<Timetable>().Update(timetable);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"timetable\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<Timetable?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT * FROM \"timetable\" WHERE \"Id\" = @id";
            return await conn.QuerySingleOrDefaultAsync<Timetable>(sql, new { id });
        }

        public async Task<bool> HasConflictForClassAsync(Guid classId, short dayOfWeek, short periodNumber)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"SELECT COUNT(*) FROM ""timetable"" 
                        WHERE ""ClassId"" = @classId AND ""DayOfWeek"" = @dayOfWeek AND ""PeriodNumber"" = @periodNumber";
            var count = await conn.ExecuteScalarAsync<int>(sql, new { classId, dayOfWeek, periodNumber });
            return count > 0;
        }

        public async Task<bool> HasConflictForTeacherAsync(Guid teacherId, short dayOfWeek, short periodNumber)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"SELECT COUNT(*) FROM ""timetable"" 
                        WHERE ""TeacherId"" = @teacherId AND ""DayOfWeek"" = @dayOfWeek AND ""PeriodNumber"" = @periodNumber";
            var count = await conn.ExecuteScalarAsync<int>(sql, new { teacherId, dayOfWeek, periodNumber });
            return count > 0;
        }

        public async Task<IEnumerable<TimetableDto>> GetByClassIdAsync(Guid classId, string? schoolYear)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        SELECT 
            t.""Id"",
            t.""ClassId"",
            c.""ClassName"",                    
            t.""SubjectId"",
            s.""Name"" AS ""SubjectName"",       
            t.""TeacherId"",
            u.""Name"" AS ""TeacherName"",         
            t.""DayOfWeek"",
            t.""PeriodNumber"",
            t.""StartTime"",
            t.""EndTime"",
            t.""Room"",
            t.""SchoolYear"",
            t.""CreatedAt"",
            t.""UpdatedAt""
        FROM ""timetable"" t
        INNER JOIN ""class"" c ON t.""ClassId"" = c.""Id""
        INNER JOIN ""subject"" s ON t.""SubjectId"" = s.""Id""
        INNER JOIN ""user"" u ON t.""TeacherId"" = u.""Id""
        WHERE t.""ClassId"" = @classId
          AND (@schoolYear IS NULL OR t.""SchoolYear"" = @schoolYear)
        ORDER BY t.""DayOfWeek"", t.""PeriodNumber""";
            return await conn.QueryAsync<TimetableDto>(sql, new { classId, schoolYear });
        }

        public async Task<IEnumerable<TimetableDto>> GetByTeacherIdAsync(Guid teacherId, string? schoolYear)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"
        SELECT 
            t.""Id"",
            t.""ClassId"",
            c.""ClassName"",                    
            t.""SubjectId"",
            s.""Name"" AS ""SubjectName"",       
            t.""TeacherId"",
            u.""Name"" AS ""TeacherName"",         
            t.""DayOfWeek"",
            t.""PeriodNumber"",
            t.""StartTime"",
            t.""EndTime"",
            t.""Room"",
            t.""SchoolYear"",
            t.""CreatedAt"",
            t.""UpdatedAt""
        FROM ""timetable"" t
        INNER JOIN ""class"" c ON t.""ClassId"" = c.""Id""
        INNER JOIN ""subject"" s ON t.""SubjectId"" = s.""Id""
        INNER JOIN ""user"" u ON t.""TeacherId"" = u.""Id""
        WHERE t.""TeacherId"" = @teacherId
        ORDER BY t.""DayOfWeek"", t.""PeriodNumber""";

            return await conn.QueryAsync<TimetableDto>(sql, new { teacherId, schoolYear });
        }

        public async Task<IEnumerable<Timetable>> GetByStudentIdAsync(Guid studentId, string? schoolYear)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"SELECT t.* FROM ""timetable"" t
                        INNER JOIN ""student_class_enrollment"" sce ON t.""ClassId"" = sce.""ClassId""
                        WHERE sce.""StudentProfileId"" = @studentId 
                          AND (@schoolYear IS NULL OR t.""SchoolYear"" = @schoolYear)
                        ORDER BY t.""DayOfWeek"", t.""PeriodNumber""";
            return await conn.QueryAsync<Timetable>(sql, new { studentId, schoolYear });
        }
    }
}