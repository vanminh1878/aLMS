using aLMS.Application.Common.Interfaces;
using aLMS.Domain.SubjectEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.SubjectInfra
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;

        public SubjectRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"Name\", \"Description\", \"Category\", \"ClassId\" FROM \"subject\"";
            return await conn.QueryAsync<Subject>(sql);
        }

        public async Task<Subject> GetSubjectByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"Name\", \"Description\", \"Category\", \"ClassId\" FROM \"subject\" WHERE \"Id\" = @id";
            return await conn.QuerySingleOrDefaultAsync<Subject>(sql, new { id });
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            await _context.Set<Subject>().AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            _context.Set<Subject>().Update(subject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubjectAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM \"subject\" WHERE \"Id\" = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> SubjectExistsAsync(Guid id)
        {
            return await _context.Set<Subject>().AnyAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(Guid classId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = "SELECT \"Id\", \"Name\", \"Description\", \"Category\", \"ClassId\" FROM \"subject\" WHERE \"ClassId\" = @classId";
            return await conn.QueryAsync<Subject>(sql, new { classId });
        }
    }
}