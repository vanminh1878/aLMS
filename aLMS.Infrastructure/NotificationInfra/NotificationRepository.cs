using aLMS.Application.Common.Interfaces;
using aLMS.Domain.NotificationEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.NotificationInfra
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public NotificationRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task AddAsync(Notification entity)
        {
            await _context.Set<Notification>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notification entity)
        {
            _context.Set<Notification>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"notification\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT n.*, u.""Name"" AS ""CreatedByName""
                FROM ""notification"" n
                INNER JOIN ""user"" u ON n.""CreatedBy"" = u.""Id""
                WHERE n.""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<Notification>(sql, new { id });
        }

        public async Task<IEnumerable<Notification>> GetByClassIdAsync(Guid classId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT n.*, u.""Name"" AS ""CreatedByName""
                FROM ""notification"" n
                INNER JOIN ""user"" u ON n.""CreatedBy"" = u.""Id""
                WHERE n.""TargetType"" = 'class' AND n.""TargetId"" = @classId
                ORDER BY n.""CreatedAt"" DESC";
            return await conn.QueryAsync<Notification>(sql, new { classId });
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT n.*, u.""Name"" AS ""CreatedByName""
                FROM ""notification"" n
                INNER JOIN ""user"" u ON n.""CreatedBy"" = u.""Id""
                WHERE n.""TargetType"" = 'user' AND n.""TargetId"" = @userId
                   OR n.""TargetType"" = 'school' -- thêm nếu cần lọc toàn trường
                ORDER BY n.""CreatedAt"" DESC";
            return await conn.QueryAsync<Notification>(sql, new { userId });
        }
    }
}