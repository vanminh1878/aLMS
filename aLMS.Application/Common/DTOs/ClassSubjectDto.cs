using System;

namespace aLMS.Application.Common.Dtos
{
    public class ClassSubjectDto
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } // Optional: từ join
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } // Optional: từ join
        public string? SchoolYear { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateClassSubjectDto
    {
        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class UpdateClassSubjectDto
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }
        public string? SchoolYear { get; set; }
    }
}