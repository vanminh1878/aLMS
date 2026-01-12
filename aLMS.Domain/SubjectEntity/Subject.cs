using aLMS.Domain.ClassEntity;
using aLMS.Domain.ClassSubjectEntity;
using aLMS.Domain.Common;
using aLMS.Domain.TopicEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.SubjectEntity
{
    public class Subject : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public ICollection<Topic> Topics { get; set; }
        public ICollection<ClassSubject> ClassSubjects { get; set; }
        public void RaiseSubjectCreatedEvent()
        {
            AddDomainEvent(new SubjectCreatedEvent(Id, Name));
        }

        public void RaiseSubjectUpdatedEvent()
        {
            AddDomainEvent(new SubjectUpdatedEvent(Id, Name));
        }

        public void RaiseSubjectDeletedEvent()
        {
            AddDomainEvent(new SubjectDeletedEvent(Id));
        }
    }
}
