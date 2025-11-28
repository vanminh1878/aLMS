using System;

namespace aLMS.Application.Common.Dtos
{
    public class TeacherProfileDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Guid? SchoolId { get; set; }
        public string? SchoolName { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Specialization { get; set; }
    }

    public class CreateTeacherProfileDto
    {
        public Guid UserId { get; set; }
        public Guid? SchoolId { get; set; }
        public Guid? DepartmentId { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Specialization { get; set; }
    }

    public class UpdateTeacherProfileDto
    {
        public Guid UserId { get; set; }
        public Guid? SchoolId { get; set; }
        public Guid? DepartmentId { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Specialization { get; set; }
    }
}