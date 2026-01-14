using aLMS.Domain.Common;
using aLMS.Domain.ClassEntity;
using aLMS.Domain.UserEntity;
using System;
using aLMS.Domain.StudentQualityEvaluationEntity;
using aLMS.Domain.StudentSubjectCommentEntity;

namespace aLMS.Domain.StudentEvaluationEntity
{
    public class StudentEvaluation : Entity
    {
        public Guid StudentId { get; set; }
        public User Student { get; set; }

        public Guid ClassId { get; set; }
        public Class Class { get; set; }

        public string Semester { get; set; } // HK1, HK2, FullYear
        public string SchoolYear { get; set; }

        public decimal? FinalScore { get; set; }
        public string Level { get; set; } // Tốt, Khá,...
        public string GeneralComment { get; set; }
        public string FinalEvaluation { get; set; } // Kết quả cuối năm

        public Guid CreatedBy { get; set; }
        public User CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<StudentSubjectComment> SubjectComments { get; set; } = new List<StudentSubjectComment>();
        public ICollection<StudentQualityEvaluation> QualityEvaluations { get; set; } = new List<StudentQualityEvaluation>();
        public void RaiseCreatedEvent()
        {
            AddDomainEvent(new StudentEvaluationCreatedEvent(Id, StudentId, ClassId));
        }

        public void RaiseUpdatedEvent()
        {
            AddDomainEvent(new StudentEvaluationUpdatedEvent(Id));
        }

        public void RaiseDeletedEvent()
        {
            AddDomainEvent(new StudentEvaluationDeletedEvent(Id));
        }
    }
}