using System;

namespace aLMS.Application.Common.Dtos
{
    public class QualityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // Yêu nước, Nhân ái,...
    }

    public class CreateQualityDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateQualityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}