// aLMS.Application.Common.Dtos/DepartmentDtos.cs
using System;

namespace aLMS.Application.Common.Dtos
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid SchoolId { get; set; }
        public string SchoolName { get; set; } = string.Empty;
    }

    public class CreateDepartmentDto
    {
        public string DepartmentName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid SchoolId { get; set; }
    }

    public class UpdateDepartmentDto
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid SchoolId { get; set; }
    }
}