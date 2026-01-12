using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.ClassSubjectEntity
{
    public class ClassSubjectCreatedEvent : IDomainEvent
    {
        public Guid ClassSubjectId { get; }
        public Guid ClassId { get; }
        public Guid SubjectId { get; }
        public string? SchoolYear { get; }

        public ClassSubjectCreatedEvent(Guid classSubjectId, Guid classId, Guid subjectId, string? schoolYear)
        {
            ClassSubjectId = classSubjectId;
            ClassId = classId;
            SubjectId = subjectId;
            SchoolYear = schoolYear;
        }
    }

    public class ClassSubjectUpdatedEvent : IDomainEvent
    {
        public Guid ClassSubjectId { get; }
        public Guid ClassId { get; }
        public Guid SubjectId { get; }
        public string? SchoolYear { get; }

        public ClassSubjectUpdatedEvent(Guid classSubjectId, Guid classId, Guid subjectId, string? schoolYear)
        {
            ClassSubjectId = classSubjectId;
            ClassId = classId;
            SubjectId = subjectId;
            SchoolYear = schoolYear;
        }
    }

    public class ClassSubjectDeletedEvent : IDomainEvent
    {
        public Guid ClassSubjectId { get; }

        public ClassSubjectDeletedEvent(Guid classSubjectId)
        {
            ClassSubjectId = classSubjectId;
        }
    }
}