using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.ClassInfra
{
    public class ClassRepository : IClassRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public ClassRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT \"Id\", \"ClassName\", \"GradeId\"");
                sb.AppendLine("FROM \"class\"");
                return await connection.QueryAsync<Class>(sb.ToString());
            }
        }

        public async Task<Class> GetClassByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT \"Id\", \"ClassName\", \"GradeId\"");
                sb.AppendLine("FROM \"class\"");
                sb.AppendLine("WHERE \"Id\" = @id");
                return await connection.QuerySingleOrDefaultAsync<Class>(sb.ToString(), new { id });
            }
        }

        public async Task AddClassAsync(Class classEntity)
        {
            await _context.Set<Class>().AddAsync(classEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClassAsync(Class classEntity)
        {
            _context.Set<Class>().Update(classEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClassAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"class\"");
                sb.AppendLine("WHERE \"Id\" = @id");
                await connection.ExecuteAsync(sb.ToString(), new { id });
            }
        }

        public async Task<IEnumerable<Class>> GetClassesByGradeIdAsync(Guid gradeId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT \"Id\", \"ClassName\", \"GradeId\"");
                sb.AppendLine("FROM \"class\"");
                sb.AppendLine("WHERE \"GradeId\" = @gradeId");
                return await connection.QueryAsync<Class>(sb.ToString(), new { gradeId });
            }
        }

        public async Task<bool> ClassExistsAsync(Guid id)
        {
            return await _context.Set<Class>().AnyAsync(c => c.Id == id);
        }
    }
}