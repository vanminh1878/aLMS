using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.SchoolEntity
{
    public class SchoolCreatedEvent : IDomainEvent
    {
        public Guid SchoolId { get; }
        public string Name { get; }
        public string Email { get; }

        public SchoolCreatedEvent(Guid schoolId, string name, string email)
        {
            SchoolId = schoolId;
            Name = name;
            Email = email;
        }
    }

    public class SchoolUpdatedEvent : IDomainEvent
    {
        public Guid SchoolId { get; }
        public string Name { get; }
        public string Email { get; }

        public SchoolUpdatedEvent(Guid schoolId, string name, string email)
        {
            SchoolId = schoolId;
            Name = name;
            Email = email;
        }
    }

    public class SchoolDeletedEvent : IDomainEvent
    {
        public Guid SchoolId { get; }

        public SchoolDeletedEvent(Guid schoolId)
        {
            SchoolId = schoolId;
        }
    }
}