using aLMS.Domain.TeacherProfileEntity;
using System;

namespace aLMS.Application.Common.Dtos
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string? DepartmentName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? HeadId { get; set; }
        public Guid? SchoolId { get; set; }
        public TeacherProfile? TeacherProfiles { get; set; }
        public int NumTeachers { get; set; }
    }

    public class CreateDepartmentDto
    {
        public string DepartmentName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? HeadId { get; set; }
    }

    public class UpdateDepartmentDto
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid SchoolId { get; set; }
    }
}