using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.VirtualClassroomEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.VirtualClassroomInfra
{
    public class VirtualClassroomRepository : IVirtualClassroomRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public VirtualClassroomRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task AddAsync(VirtualClassroom entity)
        {
            await _context.Set<VirtualClassroom>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VirtualClassroom entity)
        {
            _context.Set<VirtualClassroom>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"virtual_classroom\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<VirtualClassroom?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT vc.*, 
                       c.""ClassName"", 
                       s.""Name"" AS ""SubjectName"", 
                       u.""Name"" AS ""CreatedByName"", 
                       t.""DayOfWeek"", 
                       t.""PeriodNumber""
                FROM ""virtual_classroom"" vc
                INNER JOIN ""class"" c ON vc.""ClassId"" = c.""Id""
                LEFT JOIN ""subject"" s ON vc.""SubjectId"" = s.""Id""
                LEFT JOIN ""timetable"" t ON vc.""TimetableId"" = t.""Id""
                INNER JOIN ""teacher_profile"" tp ON vc.""CreatedBy"" = tp.""UserId""
                INNER JOIN ""user"" u ON tp.""UserId"" = u.""Id""
                WHERE vc.""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<VirtualClassroom>(sql, new { id });
        }

        public async Task<IEnumerable<VirtualClassroom>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT vc.*, 
                       c.""ClassName"", 
                       s.""Name"" AS ""SubjectName"", 
                       u.""Name"" AS ""CreatedByName"", 
                       t.""DayOfWeek"", 
                       t.""PeriodNumber""
                FROM ""virtual_classroom"" vc
                INNER JOIN ""class"" c ON vc.""ClassId"" = c.""Id""
                LEFT JOIN ""subject"" s ON vc.""SubjectId"" = s.""Id""
                LEFT JOIN ""timetable"" t ON vc.""TimetableId"" = t.""Id""
                INNER JOIN ""teacher_profile"" tp ON vc.""CreatedBy"" = tp.""UserId""
                INNER JOIN ""user"" u ON tp.""UserId"" = u.""Id""
                ORDER BY vc.""StartTime"" DESC";
            return await conn.QueryAsync<VirtualClassroom>(sql);
        }

        public async Task<IEnumerable<VirtualClassroomDto>> GetByClassIdAsync(Guid classId, bool upcomingOnly)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT vc.*, 
                       c.""ClassName"", 
                       s.""Name"" AS ""SubjectName"", 
                       u.""Name"" AS ""CreatedByName"", 
                       t.""DayOfWeek"", 
                       t.""PeriodNumber""
                FROM ""virtual_classroom"" vc
                INNER JOIN ""class"" c ON vc.""ClassId"" = c.""Id""
                LEFT JOIN ""subject"" s ON vc.""SubjectId"" = s.""Id""
                LEFT JOIN ""timetable"" t ON vc.""TimetableId"" = t.""Id""
                INNER JOIN ""teacher_profile"" tp ON vc.""CreatedBy"" = tp.""UserId""
                INNER JOIN ""user"" u ON tp.""UserId"" = u.""Id""
                WHERE vc.""ClassId"" = @classId";

            if (upcomingOnly)
            {
                sql += " AND vc.\"StartTime\" >= CURRENT_TIMESTAMP";
            }

            sql += " ORDER BY vc.\"StartTime\" ASC";

            return await conn.QueryAsync<VirtualClassroomDto>(sql, new { classId });
        }

        public async Task<IEnumerable<VirtualClassroom>> GetByStudentIdAsync(Guid studentId, bool upcomingOnly)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT vc.*, 
                       c.""ClassName"", 
                       s.""Name"" AS ""SubjectName"", 
                       u.""Name"" AS ""CreatedByName"", 
                       t.""DayOfWeek"", 
                       t.""PeriodNumber""
                FROM ""virtual_classroom"" vc
                INNER JOIN ""class"" c ON vc.""ClassId"" = c.""Id""
                LEFT JOIN ""subject"" s ON vc.""SubjectId"" = s.""Id""
                LEFT JOIN ""timetable"" t ON vc.""TimetableId"" = t.""Id""
                INNER JOIN ""teacher_profile"" tp ON vc.""CreatedBy"" = tp.""UserId""
                INNER JOIN ""user"" u ON tp.""UserId"" = u.""Id""
                INNER JOIN ""student_class_enrollment"" sce ON vc.""ClassId"" = sce.""ClassId""
                WHERE sce.""StudentProfileId"" = @studentId";

            if (upcomingOnly)
            {
                sql += " AND vc.\"StartTime\" >= CURRENT_TIMESTAMP";
            }

            sql += " ORDER BY vc.\"StartTime\" ASC";

            return await conn.QueryAsync<VirtualClassroom>(sql, new { studentId });
        }
    }
}