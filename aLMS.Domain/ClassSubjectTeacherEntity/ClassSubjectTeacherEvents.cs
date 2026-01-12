using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.ClassSubjectEntity
{
    public class ClassSubjectTeacherCreatedEvent : IDomainEvent
    {
        public Guid Id { get; }
        public Guid ClassSubjectId { get; }
        public Guid TeacherId { get; }
        public string? SchoolYear { get; }

        public ClassSubjectTeacherCreatedEvent(Guid id, Guid classSubjectId, Guid teacherId, string? schoolYear)
        {
            Id = id;
            ClassSubjectId = classSubjectId;
            TeacherId = teacherId;
            SchoolYear = schoolYear;
        }
    }

    public class ClassSubjectTeacherDeletedEvent : IDomainEvent
    {
        public Guid Id { get; }

        public ClassSubjectTeacherDeletedEvent(Guid id)
        {
            Id = id;
        }
    }
}