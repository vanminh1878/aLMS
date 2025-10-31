// aLMS.Application.Common.Dtos/RoleDtos.cs
using System;

namespace aLMS.Application.Common.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class CreateRoleDto
    {
        public string RoleName { get; set; } = string.Empty;
    }

    public class UpdateRoleDto
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}