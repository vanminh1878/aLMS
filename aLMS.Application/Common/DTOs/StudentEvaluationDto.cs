using System;
using System.Collections.Generic;

namespace aLMS.Application.Common.Dtos
{
    public class StudentEvaluationDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty; // HK1, HK2, FullYear
        public string SchoolYear { get; set; } = string.Empty;
        public decimal? FinalScore { get; set; }
        public string Level { get; set; } = string.Empty; // Tốt, Khá,...
        public string GeneralComment { get; set; } = string.Empty;
        public string FinalEvaluation { get; set; } = string.Empty; // Kết quả cuối năm
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty; // Tên GV CN
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<StudentSubjectCommentDto> SubjectComments { get; set; } = new List<StudentSubjectCommentDto>();
        public List<StudentQualityEvaluationDto> QualityEvaluations { get; set; } = new List<StudentQualityEvaluationDto>();
    }

    public class CreateStudentEvaluationDto
    {
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
        public string Semester { get; set; } = string.Empty;
        public string SchoolYear { get; set; } = string.Empty;
        public decimal? FinalScore { get; set; }
        public string Level { get; set; } = string.Empty;
        public string GeneralComment { get; set; } = string.Empty;
        public string FinalEvaluation { get; set; } = string.Empty;
    }

    public class UpdateStudentEvaluationDto
    {
        public Guid Id { get; set; }
        public decimal? FinalScore { get; set; }
        public string Level { get; set; } = string.Empty;
        public string GeneralComment { get; set; } = string.Empty;
        public string FinalEvaluation { get; set; } = string.Empty;
    }
}