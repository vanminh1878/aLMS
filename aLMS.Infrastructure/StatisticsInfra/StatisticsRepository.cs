using aLMS.Application.Common.DTOs;
using aLMS.Application.Common.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StatisticsInfra
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;
        public StatisticsRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        // 1. Số lượng giáo viên theo bộ môn
        public async Task<IEnumerable<TeacherCountByDepartmentDto>> GetTeacherCountByDepartmentAsync(Guid schoolId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT 
                    COALESCE(d.""DepartmentName"", 'Chưa phân bộ môn') AS DepartmentName,
                    COUNT(tp.""UserId"") AS TeacherCount
                FROM ""teacher_profile"" tp
                INNER JOIN ""user"" u ON tp.""UserId"" = u.""Id""
                LEFT JOIN ""department"" d ON tp.""DepartmentId"" = d.""Id""
                WHERE u.""SchoolId"" = @schoolId
                GROUP BY d.""Id"", d.""DepartmentName""
                ORDER BY TeacherCount DESC;";

            return await conn.QueryAsync<TeacherCountByDepartmentDto>(sql, new { schoolId });
        }

        // 2. Tỷ lệ hoàn thành bài tập về nhà theo lớp
        public async Task<IEnumerable<ExerciseCompletionRateDto>> GetExerciseCompletionRateByClassAsync(Guid schoolId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
        WITH class_info AS (
            SELECT 
                c.""Id"" AS class_id,
                c.""ClassName"",
                COUNT(DISTINCT sce.""StudentProfileId"") AS StudentCount
            FROM ""class"" c
            LEFT JOIN ""student_class_enrollment"" sce ON sce.""ClassId"" = c.""Id""
            WHERE c.""SchoolId"" = @schoolId 
              AND c.""IsDeleted"" = false
            GROUP BY c.""Id"", c.""ClassName""
            HAVING COUNT(DISTINCT sce.""StudentProfileId"") > 0
        ),
        total_exercises_per_class AS (
            SELECT 
                c.""Id"" AS class_id,
                COUNT(DISTINCT e.""Id"") AS TotalExercises
            FROM ""class"" c
            JOIN ""subject"" sub ON sub.""ClassId"" = c.""Id""
            JOIN ""topic"" t ON t.""SubjectId"" = sub.""Id""
            JOIN ""exercise"" e ON e.""TopicId"" = t.""Id""
            WHERE c.""SchoolId"" = @schoolId 
              AND c.""IsDeleted"" = false
            GROUP BY c.""Id""
        ),
        student_completed AS (
            SELECT 
                sce.""ClassId"" AS class_id,
                se.""StudentId"",
                COUNT(DISTINCT se.""ExerciseId"") AS CompletedCount
            FROM ""student_exercise"" se
            JOIN ""student_profile"" sp ON sp.""UserId"" = se.""StudentId""
            JOIN ""student_class_enrollment"" sce ON sce.""StudentProfileId"" = sp.""UserId""
            JOIN ""class"" c ON c.""Id"" = sce.""ClassId""
            WHERE c.""SchoolId"" = @schoolId
              AND c.""IsDeleted"" = false
              AND se.""IsCompleted"" = true
            GROUP BY sce.""ClassId"", se.""StudentId""
        ),
        completed_per_class AS (
            SELECT 
                class_id,
                ROUND(AVG(CompletedCount::numeric), 2) AS AvgCompletedPerStudent
            FROM student_completed
            GROUP BY class_id
        )
        SELECT 
            ci.""ClassName"" AS ClassName,
            COALESCE(te.TotalExercises, 0) AS TotalExercises,
            COALESCE(cp.AvgCompletedPerStudent, 0) AS AvgCompletedExercises,
            CASE 
                WHEN COALESCE(te.TotalExercises, 0) = 0 THEN 0
                ELSE ROUND((COALESCE(cp.AvgCompletedPerStudent, 0) / te.TotalExercises) * 100, 2)
            END AS CompletionRate
        FROM class_info ci
        LEFT JOIN total_exercises_per_class te ON te.class_id = ci.class_id
        LEFT JOIN completed_per_class cp ON cp.class_id = ci.class_id
        ORDER BY CompletionRate DESC NULLS LAST;";

            return await conn.QueryAsync<ExerciseCompletionRateDto>(sql, new { schoolId });
        }

        // 3. Số lượng học sinh theo khối
        public async Task<IEnumerable<StudentCountByGradeDto>> GetStudentCountByGradeAsync(Guid schoolId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT 
                    c.""Grade"",
                    COUNT(DISTINCT sce.""StudentProfileId"") AS StudentCount
                FROM ""class"" c
                JOIN ""student_class_enrollment"" sce ON sce.""ClassId"" = c.""Id""
                WHERE c.""SchoolId"" = @schoolId 
                  AND c.""IsDeleted"" = false
                GROUP BY c.""Grade""
                ORDER BY c.""Grade"";";

            return await conn.QueryAsync<StudentCountByGradeDto>(sql, new { schoolId });
        }
    }
}
