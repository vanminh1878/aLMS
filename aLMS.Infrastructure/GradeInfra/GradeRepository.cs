using aLMS.Application.Common.Interfaces;
using aLMS.Domain.GradeEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.GradeInfra
{
    public class GradeRepository : IGradeRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public GradeRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Grade>> GetAllGradesAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Grade, SchoolYear, SchoolId");
                sb.AppendLine("FROM \"Grade\"");
                return await connection.QueryAsync<Grade>(sb.ToString());
            }
        }

        public async Task<Grade> GetGradeByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Grade, SchoolYear, SchoolId");
                sb.AppendLine("FROM \"Grade\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Grade>(sb.ToString(), new { Id = id });
            }
        }

        public async Task AddGradeAsync(Grade grade)
        {
            await _context.Set<Grade>().AddAsync(grade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGradeAsync(Grade grade)
        {
            _context.Set<Grade>().Update(grade);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGradeAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Grade\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<IEnumerable<Grade>> GetGradesBySchoolIdAsync(Guid schoolId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Grade, SchoolYear, SchoolId");
                sb.AppendLine("FROM \"Grade\"");
                sb.AppendLine("WHERE SchoolId = @SchoolId");
                return await connection.QueryAsync<Grade>(sb.ToString(), new { SchoolId = schoolId });
            }
        }

        public async Task<bool> GradeExistsAsync(Guid id)
        {
            return await _context.Set<Grade>().AnyAsync(g => g.Id == id);
        }
    }
}