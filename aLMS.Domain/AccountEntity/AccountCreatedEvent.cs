using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.AccountEntity
{
    public class AccountCreatedEvent : IDomainEvent
    {
        public Guid AccountId { get; }
        public string Username { get; }

        public AccountCreatedEvent(Guid accountId, string username)
        {
            AccountId = accountId;
            Username = username;
        }
    }

    public class AccountUpdatedEvent : IDomainEvent
    {
        public Guid AccountId { get; }
        public string Username { get; }

        public AccountUpdatedEvent(Guid accountId, string username)
        {
            AccountId = accountId;
            Username = username;
        }
    }

    public class AccountDeletedEvent : IDomainEvent
    {
        public Guid AccountId { get; }

        public AccountDeletedEvent(Guid accountId)
        {
            AccountId = accountId;
        }
    }

    public class AccountDisabledEvent : IDomainEvent
    {
        public Guid AccountId { get; }

        public AccountDisabledEvent(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}