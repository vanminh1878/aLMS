// aLMS.Infrastructure/ClassInfra/ClassRepository.cs
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.ClassEntity;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;

public class ClassRepository : IClassRepository
{
    private readonly DbContext _context;
    private readonly string _connectionString;

    public ClassRepository(DbContext context, string connectionString)
    {
        _context = context;
        _connectionString = connectionString;
    }
 
    public async Task<IEnumerable<Class>> GetClassesFilteredAsync(string? grade, string? schoolYear)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = new StringBuilder("""
        SELECT "Id", "ClassName", "Grade", "SchoolYear"
        FROM "class"
        WHERE 1=1
        """);

        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(grade))
        {
            sql.AppendLine("AND \"Grade\" = @Grade");
            parameters.Add("Grade", grade.Trim());
        }

        if (!string.IsNullOrWhiteSpace(schoolYear))
        {
            sql.AppendLine("AND \"SchoolYear\" = @SchoolYear");
            parameters.Add("SchoolYear", schoolYear.Trim());
        }

        sql.AppendLine("ORDER BY \"SchoolYear\" DESC, \"Grade\", \"ClassName\"");

        return await connection.QueryAsync<Class>(sql.ToString(), parameters);
    }
    public async Task<IEnumerable<Class>> GetAllClassesAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"
            SELECT ""Id"", ""ClassName"", ""Grade"", ""SchoolYear""
            FROM ""class"" 
            ORDER BY ""SchoolYear"" DESC, ""Grade"", ""ClassName""";

        return await connection.QueryAsync<Class>(sql);
    }

    public async Task<Class?> GetClassByIdAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"
            SELECT ""Id"", ""ClassName"", ""Grade"", ""SchoolYear""
            FROM ""class""
            WHERE ""Id"" = @id";

        return await connection.QuerySingleOrDefaultAsync<Class>(sql, new { id });
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
        var entity = await _context.Set<Class>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<Class>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }


    // Infrastructure/ClassInfra/ClassRepository.cs

    public async Task SoftDeleteClassAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"
        UPDATE ""class"" 
        SET ""IsDeleted"" = true, ""DeletedAt"" = NOW() 
        WHERE ""Id"" = @id AND ""IsDeleted"" = false";

        await connection.ExecuteAsync(sql, new { id });
    }

    // Nếu muốn lấy cả lớp đã xóa (admin)
    public async Task<IEnumerable<Class>> GetAllIncludingDeletedAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Class>(
            @"SELECT ""Id"", ""ClassName"", ""Grade"", ""SchoolYear"", ""IsDeleted"", ""DeletedAt""
          FROM ""class"" ORDER BY ""SchoolYear"" DESC");
    }
    public async Task<bool> ClassExistsAsync(Guid id)
    {
        return await _context.Set<Class>().AnyAsync(c => c.Id == id);
    }
}