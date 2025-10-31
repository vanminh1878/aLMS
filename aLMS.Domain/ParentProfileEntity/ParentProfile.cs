using aLMS.Domain.Common;
using aLMS.Domain.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.ParentProfileEntity
{
    public class ParentProfile : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid StudentId { get; set; }
        public User Student { get; set; }
        public void RaiseParentProfileCreatedEvent() => AddDomainEvent(new ParentProfileCreatedEvent(UserId, StudentId));
        public void RaiseParentProfileUpdatedEvent() => AddDomainEvent(new ParentProfileUpdatedEvent(UserId, StudentId));
        public void RaiseParentProfileDeletedEvent() => AddDomainEvent(new ParentProfileDeletedEvent(UserId, StudentId));

    }

}
