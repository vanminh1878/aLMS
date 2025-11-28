using aLMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aLMS.Domain.UserEntity;

namespace aLMS.Domain.SchoolEntity
{
    public class School : Entity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; } = true;

        public ICollection<User> Users { get; set; }

        public void RaiseSchoolCreatedEvent()
        {
            AddDomainEvent(new SchoolCreatedEvent(Id, Name, Email));
        }

        public void RaiseSchoolUpdatedEvent()
        {
            AddDomainEvent(new SchoolUpdatedEvent(Id, Name, Email));
        }

        public void RaiseSchoolDeletedEvent()
        {
            AddDomainEvent(new SchoolDeletedEvent(Id));
        }
    }
}
