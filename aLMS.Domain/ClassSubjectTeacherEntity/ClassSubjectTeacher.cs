using aLMS.Domain.Common;
using aLMS.Domain.TeacherProfileEntity; 
using System;

namespace aLMS.Domain.ClassSubjectEntity
{
    public class ClassSubjectTeacher : Entity
    {
        public Guid ClassSubjectId { get; private set; }
        public ClassSubject ClassSubject { get; private set; } = null!;

        public Guid TeacherId { get; private set; }
        public TeacherProfile Teacher { get; private set; } = null!;

        public string? SchoolYear { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private ClassSubjectTeacher() { }

        public ClassSubjectTeacher(Guid classSubjectId, Guid teacherId, string? schoolYear = null)
        {
            ClassSubjectId = classSubjectId;
            TeacherId = teacherId;
            SchoolYear = schoolYear;

            RaiseCreatedEvent();
        }

        public void RaiseCreatedEvent()
        {
            AddDomainEvent(new ClassSubjectTeacherCreatedEvent(Id, ClassSubjectId, TeacherId, SchoolYear));
        }

        public void RaiseDeletedEvent()
        {
            AddDomainEvent(new ClassSubjectTeacherDeletedEvent(Id));
        }
    }
}