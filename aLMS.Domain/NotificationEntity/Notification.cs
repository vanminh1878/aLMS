using aLMS.Domain.Common;
using aLMS.Domain.UserEntity;
using System;

namespace aLMS.Domain.NotificationEntity
{
    public class Notification : Entity
    {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string TargetType { get; private set; } // school, class, student, virtual_classroom
        public Guid? TargetId { get; private set; }
        public Guid CreatedBy { get; private set; }
        public User CreatedByUser { get; private set; } = null!;
        public Guid? SchoolId { get; private set; }
        public bool IsRead { get; private set; } = false;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private Notification() { }

        public Notification(
            string title,
            string content,
            string targetType,
            Guid? targetId,
            Guid createdBy,
            Guid? schoolId = null)
        {
            Title = title;
            Content = content;
            TargetType = targetType;
            TargetId = targetId;
            CreatedBy = createdBy;
            SchoolId = schoolId;

            RaiseCreatedEvent();
        }

        public void MarkAsRead()
        {
            IsRead = true;
            RaiseUpdatedEvent();
        }

        public void RaiseCreatedEvent()
        {
            AddDomainEvent(new NotificationCreatedEvent(Id, Title, TargetType));
        }

        public void RaiseUpdatedEvent()
        {
            AddDomainEvent(new NotificationUpdatedEvent(Id));
        }
    }
}