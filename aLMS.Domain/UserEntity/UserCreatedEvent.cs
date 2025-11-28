using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.UserEntity
{
    public class UserCreatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public string Name { get; }
        public Guid? AccountId { get; }
        public Guid? RoleId { get; }

        public UserCreatedEvent(Guid userId, string name, Guid? accountId, Guid? roleId)
        {
            UserId = userId;
            Name = name;
            AccountId = accountId;
            RoleId = roleId;
        }
    }

    public class UserUpdatedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public string Name { get; }
        public Guid? RoleId { get; }

        public UserUpdatedEvent(Guid userId, string name, Guid? roleId)
        {
            UserId = userId;
            Name = name;
            RoleId = roleId;
        }
    }

    public class UserDeletedEvent : IDomainEvent
    {
        public Guid UserId { get; }

        public UserDeletedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}