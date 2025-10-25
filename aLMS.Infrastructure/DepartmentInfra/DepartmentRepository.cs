using aLMS.Application.Common.Interfaces;
using aLMS.Domain.DepartmentEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, DepartmentName, HeadId");
                sb.AppendLine("FROM \"Department\"");
                return await connection.QueryAsync<Department>(sb.ToString());
            }
        }

        public async Task<Department> GetDepartmentByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, DepartmentName, HeadId");
                sb.AppendLine("FROM \"Department\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Department>(sb.ToString(), new { Id = id });
            }
        }

        public async Task AddDepartmentAsync(Department department)
        {
            await _context.Set<Department>().AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            _context.Set<Department>().Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Department\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<bool> DepartmentExistsAsync(Guid id)
        {
            return await _context.Set<Department>().AnyAsync(d => d.Id == id);
        }
    }
}