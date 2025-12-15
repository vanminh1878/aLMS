namespace aLMS.Application.Common.Dtos
{
    public class ClassDto
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;        
        public string SchoolYear { get; set; } = string.Empty;
        public bool IsDelete { get; set; }
        public int NumStudent { get; set; }
        public string homeroomTeacherName { get; set; }
    }

    public class CreateClassDto
    {
        public string ClassName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string SchoolId { get; set; } = string.Empty;
        public string SchoolYear { get; set; } = string.Empty;
    }

    public class UpdateClassDto
    {
        public Guid Id { get; set; }
        public Guid SchoolId { get; set; }
        public string? ClassName { get; set; } = string.Empty;
        public string? Grade { get; set; } = string.Empty;
        public string? SchoolYear { get; set; } = string.Empty;
        public bool? IsDelete { get; set; }
        public Guid? HomeroomTeacherId { get; set; }
    }


    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static OperationResult SuccessResult() => new() { Success = true };
        public static OperationResult Fail(string message) => new() { Success = false, Message = message };
    }
}