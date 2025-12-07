// aLMS.Infrastructure.DepartmentInfra/DepartmentRepository.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.DepartmentEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.DepartmentInfra
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public DepartmentRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllBySchoolIdAsync(Guid schoolId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT d.*
                FROM ""department"" d
                JOIN ""school"" s ON d.""SchoolId"" = s.""Id""
                WHERE d.""SchoolId"" = @schoolId";
            return await conn.QueryAsync<DepartmentDto>(sql, new { schoolId });
        }

        public async Task<Department?> GetByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT d.*, s.""Name"" as SchoolName
                FROM ""department"" d
                JOIN ""school"" s ON d.""SchoolId"" = s.""Id""
                WHERE d.""Id"" = @id";
            return await conn.QuerySingleOrDefaultAsync<Department>(sql, new { id });
        }

        public async Task AddAsync(Department department)
        {
            await _context.Set<Department>().AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Set<Department>().Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"department\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT COUNT(1) FROM \"department\" WHERE \"Id\" = @id";
            return await conn.ExecuteScalarAsync<int>(sql, new { id }) > 0;
        }

        public async Task<bool> NameExistsInSchoolAsync(string name, Guid? excludeId = null)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = excludeId.HasValue
                ? "SELECT COUNT(1) FROM \"department\" WHERE \"DepartmentName\" = @name AND \"Id\" != @excludeId"
                : "SELECT COUNT(1) FROM \"department\" WHERE \"DepartmentName\" = @name ";
            return await conn.ExecuteScalarAsync<int>(sql, new { name, excludeId }) > 0;
        }
    }
}