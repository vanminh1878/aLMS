using aLMS.Domain.ClassEntity;
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

        public Guid ClassId { get; set; }
        public Class Class { get; set; }

        public ICollection<Topic> Topics { get; set; }
        public void RaiseSubjectCreatedEvent()
        {
            AddDomainEvent(new SubjectCreatedEvent(Id, Name, ClassId));
        }

        public void RaiseSubjectUpdatedEvent()
        {
            AddDomainEvent(new SubjectUpdatedEvent(Id, Name, ClassId));
        }

        public void RaiseSubjectDeletedEvent()
        {
            AddDomainEvent(new SubjectDeletedEvent(Id));
        }
    }
}
