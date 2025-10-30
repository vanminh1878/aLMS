using aLMS.Domain.ClassEntity;
using aLMS.Domain.Common;
using aLMS.Domain.SchoolEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.GradeEntity
{
    public class Grade : Entity
    {
        public string GradeName { get; set; }
        public string SchoolYear { get; set; }

        public Guid SchoolId { get; set; }
        public SchoolEntity.School School { get; set; }

        public ICollection<Class> Classes { get; set; }

        public void RaiseGradeCreatedEvent()
        {
            AddDomainEvent(new GradeCreatedEvent(Id, GradeName, SchoolYear, SchoolId));
        }

        public void RaiseGradeUpdatedEvent()
        {
            AddDomainEvent(new GradeUpdatedEvent(Id, GradeName, SchoolYear, SchoolId));
        }

        public void RaiseGradeDeletedEvent()
        {
            AddDomainEvent(new GradeDeletedEvent(Id));
        }
    }
}