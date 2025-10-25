using aLMS.Application.Common.Interfaces;
using aLMS.Domain.SchoolEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.SchoolInfra
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public SchoolRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<School>> GetAllSchoolsAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT \"Id\", \"Name\", \"Address\", \"Email\", \"Status\"");
                sb.AppendLine("FROM \"school\"");
                return await connection.QueryAsync<School>(sb.ToString());
            }
        }

        public async Task<School> GetSchoolByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT \"Id\", \"Name\", \"Address\", \"Email\", \"Status\"");
                sb.AppendLine("FROM \"school\"");
                sb.AppendLine("WHERE \"Id\" = @id");
                return await connection.QuerySingleOrDefaultAsync<School>(sb.ToString(), new { id });
            }
        }

        public async Task AddSchoolAsync(School school)
        {
            await _context.Set<School>().AddAsync(school);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSchoolAsync(School school)
        {
            _context.Set<School>().Update(school);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSchoolAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"school\"");
                sb.AppendLine("WHERE \"Id\" = @id");
                await connection.ExecuteAsync(sb.ToString(), new { id });
            }
        }

        public async Task<bool> SchoolExistsAsync(Guid id)
        {
            return await _context.Set<School>().AnyAsync(s => s.Id == id);
        }
    }
}
