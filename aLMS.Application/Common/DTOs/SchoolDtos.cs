using System;

namespace aLMS.Application.Common.Dtos
{
    public class SchoolDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Status { get; set; }
    }

    public class CreateSchoolDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public string AdminName { get; set; } = string.Empty;
        public string AdminUsername { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
        public string? AdminAddress { get; set; }
        public string? AdminEmail { get; set; }
        public string? AdminPhone { get; set; }
        public DateTime? AdminDateOfBirth { get; set; }
        public string? AdminGender { get; set; }
        public Guid RoleId { get; set; }
    }

    public class UpdateSchoolDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}