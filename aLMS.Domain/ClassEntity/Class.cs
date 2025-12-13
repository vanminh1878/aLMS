// aLMS.Domain/ClassEntity/Class.cs
using aLMS.Domain.Common;
using aLMS.Domain.SubjectEntity;
using aLMS.Domain.TeacherProfileEntity;
using System;

namespace aLMS.Domain.ClassEntity
{
    public class Class : Entity
    {
        public string ClassName { get; set; } = null!;
        public string Grade { get; set; } = null!;
        public string SchoolYear { get; set; } = null!;

        public Guid SchoolId { get; set; }
        public Guid? HomeroomTeacherId { get; set; } 

        public TeacherProfile? HomeroomTeacher { get; set; }

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // Domain events
        public void RaiseClassCreatedEvent()
            => AddDomainEvent(new ClassCreatedEvent(Id, ClassName, Grade, SchoolYear, SchoolId));

        public void RaiseClassUpdatedEvent()
            => AddDomainEvent(new ClassUpdatedEvent(Id, ClassName, Grade, SchoolYear, SchoolId));

        public void SoftDelete()
        {
            if (IsDeleted) return;

            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            AddDomainEvent(new ClassSoftDeletedEvent(Id));
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}