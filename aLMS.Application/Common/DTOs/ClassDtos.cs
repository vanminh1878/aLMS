using System;

namespace aLMS.Application.Common.Dtos
{
    public class ClassDto
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Guid GradeId { get; set; }
    }

    public class CreateClassDto
    {
        public string ClassName { get; set; } = string.Empty;
        public Guid GradeId { get; set; }
    }

    public class UpdateClassDto
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Guid GradeId { get; set; }
    }
    public class DeleteClassResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}