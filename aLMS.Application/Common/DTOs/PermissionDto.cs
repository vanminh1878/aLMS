using System;

namespace aLMS.Application.Common.Dtos
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string PermissionName { get; set; } = string.Empty;
    }

    public class CreatePermissionDto
    {
        public string PermissionName { get; set; } = string.Empty;
    }

    public class UpdatePermissionDto
    {
        public Guid Id { get; set; }
        public string PermissionName { get; set; } = string.Empty;
    }
}