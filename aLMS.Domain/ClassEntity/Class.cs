using aLMS.Domain.Common;
using aLMS.Domain.SubjectEntity;

namespace aLMS.Domain.ClassEntity
{
    public class Class : Entity
    {
        public string ClassName { get; set; } = null!;
        public string Grade { get; set; } = null!;
        public string SchoolYear { get; set; } = null!;

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // Domain events
        public void RaiseClassCreatedEvent()
            => AddDomainEvent(new ClassCreatedEvent(Id, ClassName, Grade, SchoolYear));

        public void RaiseClassUpdatedEvent()
            => AddDomainEvent(new ClassUpdatedEvent(Id, ClassName, Grade, SchoolYear));

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