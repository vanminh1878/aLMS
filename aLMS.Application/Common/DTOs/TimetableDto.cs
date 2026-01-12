using System;

namespace aLMS.Application.Common.Dtos
{
    public class TimetableDto
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }
        public short DayOfWeek { get; set; }
        public short PeriodNumber { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Room { get; set; }
        public string? SchoolYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateTimetableDto
    {
        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid TeacherId { get; set; }
        public short DayOfWeek { get; set; }
        public short PeriodNumber { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Room { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class UpdateTimetableDto
    {
        public Guid Id { get; set; }
        public short DayOfWeek { get; set; }
        public short PeriodNumber { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Room { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class GenerateTimetableDto
    {
        public Guid ClassId { get; set; }
        public string SchoolYear { get; set; }
        public short NumberOfPeriodsPerDay { get; set; } = 10; // Số tiết/ngày
        public short MaxRetries { get; set; } = 50; // Số lần thử random
    }

    public class GenerateTimetableResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int AssignedCount { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
    }
}