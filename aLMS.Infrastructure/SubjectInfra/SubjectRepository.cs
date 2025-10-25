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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Name, Description, Category, ClassId");
                sb.AppendLine("FROM \"Subject\"");
                return await connection.QueryAsync<Subject>(sb.ToString());
            }
        }

        public async Task<Subject> GetSubjectByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Name, Description, Category, ClassId");
                sb.AppendLine("FROM \"Subject\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Subject>(sb.ToString(), new { Id = id });
            }
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Subject\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(Guid classId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Name, Description, Category, ClassId");
                sb.AppendLine("FROM \"Subject\"");
                sb.AppendLine("WHERE ClassId = @ClassId");
                return await connection.QueryAsync<Subject>(sb.ToString(), new { ClassId = classId });
            }
        }

        public async Task<bool> SubjectExistsAsync(Guid id)
        {
            return await _context.Set<Subject>().AnyAsync(s => s.Id == id);
        }
    }
}