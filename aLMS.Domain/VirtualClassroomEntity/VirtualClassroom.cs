using aLMS.Domain.Common;
using aLMS.Domain.ClassEntity;
using aLMS.Domain.SubjectEntity;
using aLMS.Domain.TeacherProfileEntity;
using aLMS.Domain.TimetableEntity;
using System;

namespace aLMS.Domain.VirtualClassroomEntity
{
    public class VirtualClassroom : Entity
    {
        public Guid ClassId { get; private set; }
        public Class Class { get; private set; } = null!;

        public Guid? SubjectId { get; private set; }
        public Subject? Subject { get; private set; }

        public Guid? TimetableId { get; private set; }
        public Timetable? Timetable { get; private set; }

        public short? DayOfWeek { get; private set; }
        public short? PeriodNumber { get; private set; }

        public string Title { get; private set; }
        public string MeetingUrl { get; private set; }
        public string? MeetingId { get; private set; }
        public string? Password { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public Guid CreatedBy { get; private set; }
        public TeacherProfile CreatedByTeacher { get; private set; } = null!;

        public bool IsRecurring { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        private VirtualClassroom() { }

        public VirtualClassroom(
            Guid classId,
            string title,
            string meetingUrl,
            DateTime startTime,
            DateTime endTime,
            Guid createdBy,
            bool isRecurring = false,
            Guid? subjectId = null,
            Guid? timetableId = null,
            short? dayOfWeek = null,
            short? periodNumber = null,
            string? meetingId = null,
            string? password = null)
        {
            ClassId = classId;
            Title = title;
            MeetingUrl = meetingUrl;
            StartTime = startTime;
            EndTime = endTime;
            CreatedBy = createdBy;
            IsRecurring = isRecurring;

            SubjectId = subjectId;
            TimetableId = timetableId;
            DayOfWeek = dayOfWeek;
            PeriodNumber = periodNumber;
            MeetingId = meetingId;
            Password = password;

            RaiseCreatedEvent();
        }

        public void RaiseCreatedEvent()
        {
            AddDomainEvent(new VirtualClassroomCreatedEvent(Id, ClassId, Title));
        }

        public void RaiseUpdatedEvent()
        {
            AddDomainEvent(new VirtualClassroomUpdatedEvent(Id));
        }

        public void RaiseDeletedEvent()
        {
            AddDomainEvent(new VirtualClassroomDeletedEvent(Id));
        }
    }
}