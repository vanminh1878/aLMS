using aLMS.Domain.Common;
using aLMS.Domain.ClassEntity;
using aLMS.Domain.SubjectEntity;
using System;
namespace aLMS.Domain.ClassSubjectEntity
{
    public class ClassSubject : Entity
    {
        public Guid ClassId { get; private set; }
        public Class Class { get; private set; } = null!; public Guid SubjectId { get; private set; }
        public Subject Subject { get; private set; } = null!;

        public string? SchoolYear { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        private ClassSubject() { }

        public ClassSubject(Guid classId, Guid subjectId, string? schoolYear = null)
        {
            ClassId = classId;
            SubjectId = subjectId;
            SchoolYear = schoolYear;

            RaiseCreatedEvent();
        }

        public void RaiseCreatedEvent()
        {
            AddDomainEvent(new ClassSubjectCreatedEvent(Id, ClassId, SubjectId, SchoolYear));
        }

        public void RaiseUpdatedEvent()
        {
            AddDomainEvent(new ClassSubjectUpdatedEvent(Id, ClassId, SubjectId, SchoolYear));
        }

        public void RaiseDeletedEvent()
        {
            AddDomainEvent(new ClassSubjectDeletedEvent(Id));
        }
    }
}

