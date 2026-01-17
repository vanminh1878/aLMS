using System;

namespace aLMS.Application.Common.Dtos
{
    public class VirtualClassroomDto
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Guid? SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public Guid? TimetableId { get; set; }
        public short? DayOfWeek { get; set; }
        public short? PeriodNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MeetingUrl { get; set; } = string.Empty;
        public string? MeetingId { get; set; }
        public string? Password { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public bool IsRecurring { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateVirtualClassroomDto
    {
        public Guid ClassId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? TimetableId { get; set; }
        public short? DayOfWeek { get; set; } // Bắt buộc nếu TimetableId null
        public short? PeriodNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MeetingUrl { get; set; } = string.Empty;
        public string? MeetingId { get; set; }
        public string? Password { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsRecurring { get; set; }
    }

    public class UpdateVirtualClassroomDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MeetingUrl { get; set; } = string.Empty;
        public string? MeetingId { get; set; }
        public string? Password { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsRecurring { get; set; }
    }
}