using System;

namespace aLMS.Application.Common.Dtos
{
    public class ClassSubjectTeacherDto
    {
        public Guid Id { get; set; }
        public Guid ClassSubjectId { get; set; }
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string Subject_Name { get; set; }
        public string? SchoolYear { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AddClassSubjectTeacherDto
    {
        public Guid TeacherId { get; set; }
        public string? SchoolYear { get; set; }
    }
}