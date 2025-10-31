// aLMS.Application.Common.Dtos/StudentProfileDtos.cs
using System;

namespace aLMS.Application.Common.Dtos
{
    public class StudentProfileDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Guid? SchoolId { get; set; }
        public string? SchoolName { get; set; }
        public Guid? ClassId { get; set; }
        public string? ClassName { get; set; }
        public DateTime? EnrollDate { get; set; }
    }

    public class CreateStudentProfileDto
    {
        public Guid UserId { get; set; }
        public Guid? SchoolId { get; set; }
        public Guid? ClassId { get; set; }
        public DateTime? EnrollDate { get; set; }
    }

    public class UpdateStudentProfileDto
    {
        public Guid UserId { get; set; }
        public Guid? SchoolId { get; set; }
        public Guid? ClassId { get; set; }
        public DateTime? EnrollDate { get; set; }
    }
}