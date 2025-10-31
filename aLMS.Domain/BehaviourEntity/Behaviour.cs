using aLMS.Domain.Common;
using aLMS.Domain.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.BehaviourEntity
{
    public class Behaviour : Entity
    {
        public Guid StudentId { get; set; }
        public  User Student { get; set; }

        public string Video { get; set; }
        public string Result { get; set; }
        public int Order { get; set; }
        public DateTime Date { get; set; }
        public void RaiseBehaviourCreatedEvent() => AddDomainEvent(new BehaviourCreatedEvent(Id, StudentId, Order, Date));
        public void RaiseBehaviourUpdatedEvent() => AddDomainEvent(new BehaviourUpdatedEvent(Id, StudentId, Order, Date));
        public void RaiseBehaviourDeletedEvent() => AddDomainEvent(new BehaviourDeletedEvent(Id));
    }
}
