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
    public async Task<IEnumerable<Class>> GetClassesBySchoolIdAsync(Guid schoolId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"
        SELECT c.""Id"", c.""ClassName"", c.""Grade"", c.""SchoolYear"", c.""HomeroomTeacherId""
        FROM ""class"" c
        WHERE c.""SchoolId"" = @schoolId
        ORDER BY c.""Grade"", c.""ClassName""";

        return await connection.QueryAsync<Class>(sql, new { schoolId });
    }
    public async Task<Class?> GetClassByHomeroomTeacherIdAsync(Guid homeroomTeacherId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"
        SELECT *
        FROM ""class""
        WHERE ""HomeroomTeacherId"" = @homeroomTeacherId
          AND ""IsDeleted"" = false";

        return await connection.QuerySingleOrDefaultAsync<Class>(sql, new { homeroomTeacherId });
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

    public async Task SoftDeleteClassAsync(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"
        UPDATE ""class"" 
        SET ""IsDeleted"" = true, ""DeletedAt"" = NOW() 
        WHERE ""Id"" = @id AND ""IsDeleted"" = false";

        await connection.ExecuteAsync(sql, new { id });
    }

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
    public async Task<bool> ClassNameExistsAsync(string classname, Guid? excludeId = null)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        var sql = excludeId.HasValue
            ? "SELECT COUNT(1) FROM \"class\" WHERE \"ClassName\" = @classname AND \"Id\" != @excludeId"
            : "SELECT COUNT(1) FROM \"class\" WHERE \"ClassName\" = @classname";
        return await conn.ExecuteScalarAsync<int>(sql, new { classname, excludeId }) > 0;
    }
    public async Task<IEnumerable<Class>> GetClassesByStudentIdAsync(Guid studentId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var sql = @"
        SELECT DISTINCT c.""Id"", c.""ClassName"", c.""Grade"", c.""SchoolYear"", 
               c.""SchoolId"", c.""HomeroomTeacherId"", c.""IsDeleted"", c.""DeletedAt""
        FROM ""student_class_enrollment"" sce
        JOIN ""class"" c ON sce.""ClassId"" = c.""Id""
        WHERE sce.""StudentProfileId"" = @studentId
          AND c.""IsDeleted"" = false
        ORDER BY c.""SchoolYear"" DESC, c.""Grade"", c.""ClassName""";

        return await connection.QueryAsync<Class>(sql, new { studentId });
    }
}