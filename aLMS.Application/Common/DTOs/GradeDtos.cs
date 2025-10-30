using System;

namespace aLMS.Application.Common.Dtos
{
    public class GradeDto
    {
        public Guid Id { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public string SchoolYear { get; set; } = string.Empty;
        public Guid SchoolId { get; set; }
    }

    public class CreateGradeDto
    {
        public string GradeName { get; set; } = string.Empty;
        public string SchoolYear { get; set; } = string.Empty;
        public Guid SchoolId { get; set; }
    }

    public class UpdateGradeDto
    {
        public Guid Id { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public string SchoolYear { get; set; } = string.Empty;
        public Guid SchoolId { get; set; }
    }
}