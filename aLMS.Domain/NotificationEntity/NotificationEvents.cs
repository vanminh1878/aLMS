using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.NotificationEntity
{
    public class NotificationCreatedEvent : IDomainEvent
    {
        public Guid Id { get; }
        public string Title { get; }
        public string TargetType { get; }

        public NotificationCreatedEvent(Guid id, string title, string targetType)
        {
            Id = id;
            Title = title;
            TargetType = targetType;
        }
    }

    public class NotificationUpdatedEvent : IDomainEvent
    {
        public Guid Id { get; }

        public NotificationUpdatedEvent(Guid id)
        {
            Id = id;
        }
    }
}