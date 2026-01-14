using System;

namespace aLMS.Application.Common.Dtos
{
    public class StudentQualityEvaluationDto
    {
        public Guid Id { get; set; }
        public Guid StudentEvaluationId { get; set; }
        public Guid QualityId { get; set; }
        public string QualityName { get; set; } = string.Empty;
        public string QualityLevel { get; set; } = string.Empty; // Tốt, Khá,...
    }

    public class CreateStudentQualityEvaluationDto
    {
        public Guid StudentEvaluationId { get; set; }
        public Guid QualityId { get; set; }
        public string QualityLevel { get; set; } = string.Empty;
    }
}