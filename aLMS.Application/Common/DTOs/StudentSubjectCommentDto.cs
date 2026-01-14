using System;

namespace aLMS.Application.Common.Dtos
{
    public class StudentSubjectCommentDto
    {
        public Guid Id { get; set; }
        public Guid StudentEvaluationId { get; set; }
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }

    public class CreateStudentSubjectCommentDto
    {
        public Guid StudentEvaluationId { get; set; }
        public Guid SubjectId { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}