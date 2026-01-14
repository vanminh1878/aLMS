using System;

namespace aLMS.Application.Common.Dtos
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string TargetType { get; set; } = string.Empty; // school, class, student, virtual_classroom
        public Guid? TargetId { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public Guid? SchoolId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateNotificationDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string TargetType { get; set; } = string.Empty;
        public Guid? TargetId { get; set; }
        public Guid? SchoolId { get; set; }
    }

    public class UpdateNotificationDto
    {
        public Guid Id { get; set; }
        public bool IsRead { get; set; }
    }
}