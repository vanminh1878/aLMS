// aLMS.Infrastructure.BehaviourInfra/BehaviourRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.BehaviourEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.BehaviourInfra
{
    public class BehaviourRepository : IBehaviourRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public BehaviourRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Behaviour>> GetByStudentIdAsync(Guid studentId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT b.*, u.""Name"" as StudentName
                FROM ""behaviour"" b
                JOIN ""user"" u ON b.""StudentId"" = u.""Id""
                WHERE b.""StudentId"" = @studentId
                ORDER BY b.""Order""";
            return await conn.QueryAsync<Behaviour>(sql, new { studentId });
        }

        public async Task<Behaviour?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT b.*, u.""Name"" as StudentName
                FROM ""behaviour"" b
                JOIN ""user"" u ON b.""StudentId"" = u.""Id""
                WHERE b.""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<Behaviour>(sql, new { id });
        }

        public async Task AddAsync(Behaviour behaviour)
        {
            await _context.Set<Behaviour>().AddAsync(behaviour);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Behaviour behaviour)
        {
            _context.Set<Behaviour>().Update(behaviour);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"behaviour\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM \"behaviour\" WHERE \"Id\" = @id";
            return await conn.ExecuteScalarAsync<int>(sql, new { id }) > 0;
        }

        public async Task<int> GetNextOrderAsync(Guid studentId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT COALESCE(MAX(\"Order\"), 0) + 1 FROM \"behaviour\" WHERE \"StudentId\" = @studentId";
            return await conn.ExecuteScalarAsync<int>(sql, new { studentId });
        }
    }
}