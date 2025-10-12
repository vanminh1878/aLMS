using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        protected Entity() { }
        protected Entity(Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }
        protected readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public IReadOnlyCollection<IDomainEvent> PopDomainEvents()
        {
            var domainEvents = _domainEvents;
            _domainEvents.Clear();
            return domainEvents;
        }

    }
}
