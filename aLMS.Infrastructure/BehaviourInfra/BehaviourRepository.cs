using aLMS.Application.Common.Interfaces;
using aLMS.Domain.BehaviourEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<IEnumerable<Behaviour>> GetAllBehavioursAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Video, Result, Order, StudentId");
                sb.AppendLine("FROM \"Behaviour\"");
                return await connection.QueryAsync<Behaviour>(sb.ToString());
            }
        }

        public async Task<Behaviour> GetBehaviourByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Video, Result, Order, StudentId");
                sb.AppendLine("FROM \"Behaviour\"");
                sb.AppendLine("WHERE Id = @Id");
                return await connection.QuerySingleOrDefaultAsync<Behaviour>(sb.ToString(), new { Id = id });
            }
        }

        public async Task AddBehaviourAsync(Behaviour behaviour)
        {
            await _context.Set<Behaviour>().AddAsync(behaviour);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBehaviourAsync(Behaviour behaviour)
        {
            _context.Set<Behaviour>().Update(behaviour);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBehaviourAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM \"Behaviour\"");
                sb.AppendLine("WHERE Id = @Id");
                await connection.ExecuteAsync(sb.ToString(), new { Id = id });
            }
        }

        public async Task<IEnumerable<Behaviour>> GetBehavioursByStudentIdAsync(Guid studentId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Video, Result, Order, StudentId");
                sb.AppendLine("FROM \"Behaviour\"");
                sb.AppendLine("WHERE StudentId = @StudentId");
                return await connection.QueryAsync<Behaviour>(sb.ToString(), new { StudentId = studentId });
            }
        }

        public async Task<bool> BehaviourExistsAsync(Guid id)
        {
            return await _context.Set<Behaviour>().AnyAsync(b => b.Id == id);
        }
    }
}