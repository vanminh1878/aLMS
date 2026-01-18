using aLMS.Domain.Common;
using aLMS.Domain.StudentProfileEntity;
using System;

namespace aLMS.Domain.StudentFinalTermRecordEntity
{
    public class StudentFinalTermRecord : Entity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; } = null!;
        public Guid? ClassId { get; set; }
        // public Guid? SchoolYearId { get; set; }  // tùy chọn
        public Guid SubjectId { get; set; }
        public decimal? FinalScore { get; set; }   
        public string? FinalEvaluation { get; set; }      
        public string? Comment { get; set; }              

        public DateTime? CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }

        // Domain Events (tùy chọn)
        public void RaiseRecordCreatedEvent()
            => AddDomainEvent(new StudentFinalTermRecordCreatedEvent(Id, StudentProfileId));

        public void RaiseRecordUpdatedEvent()
            => AddDomainEvent(new StudentFinalTermRecordUpdatedEvent(Id, StudentProfileId));
    }

    // Các domain event (tùy chọn)
    public record StudentFinalTermRecordCreatedEvent(Guid RecordId, Guid StudentProfileId) : IDomainEvent;
    public record StudentFinalTermRecordUpdatedEvent(Guid RecordId, Guid StudentProfileId) : IDomainEvent;
}