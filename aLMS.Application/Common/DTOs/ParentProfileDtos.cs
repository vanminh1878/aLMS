// aLMS.Application.Common.Dtos/ParentProfileDtos.cs
using System;

namespace aLMS.Application.Common.Dtos
{
    public class ParentProfileDto
    {
        public Guid ParentId { get; set; }
        public string ParentName { get; set; } = string.Empty;
        public string? ParentEmail { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? StudentEmail { get; set; }
    }

    public class CreateParentProfileDto
    {
        public Guid ParentId { get; set; }
        public Guid StudentId { get; set; }
    }

    public class UpdateParentProfileDto
    {
        public Guid ParentId { get; set; }
        public Guid StudentId { get; set; }
    }
}