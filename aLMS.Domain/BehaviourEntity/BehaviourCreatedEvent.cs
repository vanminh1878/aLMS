using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.BehaviourEntity
{
    public class BehaviourCreatedEvent : IDomainEvent
    {
        public Guid BehaviourId { get; }
        public Guid StudentId { get; }
        public int Order { get; }
        public DateTime Date { get; }

        public BehaviourCreatedEvent(Guid behaviourId, Guid studentId, int order, DateTime date)
        {
            BehaviourId = behaviourId;
            StudentId = studentId;
            Order = order;
            Date = date;
        }
    }

    public class BehaviourUpdatedEvent : IDomainEvent
    {
        public Guid BehaviourId { get; }
        public Guid StudentId { get; }
        public int Order { get; }
        public DateTime Date { get; }

        public BehaviourUpdatedEvent(Guid behaviourId, Guid studentId, int order, DateTime date)
        {
            BehaviourId = behaviourId;
            StudentId = studentId;
            Order = order;
            Date = date;
        }
    }

    public class BehaviourDeletedEvent : IDomainEvent
    {
        public Guid BehaviourId { get; }

        public BehaviourDeletedEvent(Guid behaviourId)
        {
            BehaviourId = behaviourId;
        }
    }
}