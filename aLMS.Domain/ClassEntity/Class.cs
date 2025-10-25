using aLMS.Domain.Common;
using aLMS.Domain.GradeEntity;
using aLMS.Domain.SubjectEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.ClassEntity
{
    public class Class : Entity
    {
        public string ClassName { get; set; }

        public Guid GradeId { get; set; }
        public Grade Grade { get; set; }

        public ICollection<Subject> Subjects { get; set; }

        public void RaiseClassCreatedEvent()
        {
            AddDomainEvent(new ClassCreatedEvent(Id, ClassName, GradeId));
        }

        public void RaiseClassUpdatedEvent()
        {
            AddDomainEvent(new ClassUpdatedEvent(Id, ClassName, GradeId));
        }

        public void RaiseClassDeletedEvent()
        {
            AddDomainEvent(new ClassDeletedEvent(Id));
        }
    }
}
