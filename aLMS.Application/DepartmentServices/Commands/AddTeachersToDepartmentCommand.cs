// AddTeachersToDepartmentCommand.cs
using MediatR;

public class AddTeachersToDepartmentCommand : IRequest<AddTeachersToDepartmentResult>
{
    public Guid DepartmentId { get; set; }
    public List<AddTeacherToDepartmentDto> Teachers { get; set; } = new();
}

public class AddTeacherToDepartmentDto
{
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = "Nam";
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime HireDate { get; set; }
    public string? Specialization { get; set; }
    public Guid SchoolId { get; set; }
}

public class AddTeachersToDepartmentResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int SuccessCount => CreatedTeachers.Count;
    public int ErrorCount => Errors.Count;
    public List<TeacherCreationResult> CreatedTeachers { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}

public class TeacherCreationResult
{
    public string TeacherName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}