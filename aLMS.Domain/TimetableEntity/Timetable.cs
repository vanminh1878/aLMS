using aLMS.Domain.ClassEntity;
using aLMS.Domain.Common;
using aLMS.Domain.SubjectEntity;
using aLMS.Domain.TeacherProfileEntity;
using System;

namespace aLMS.Domain.TimetableEntity
{
    public class Timetable : Entity
    {
        public Guid ClassId { get; private set; }
        public Class Class { get; private set; } = null!;

        public Guid SubjectId { get; private set; }
        public Subject Subject { get; private set; } = null!;

        public Guid TeacherId { get; private set; }
        public TeacherProfile Teacher { get; private set; } = null!;

        public short DayOfWeek { get; private set; } // 1=Thứ 2 → 7=CN
        public short PeriodNumber { get; private set; } // tiết 1,2,...

        public TimeSpan? StartTime { get; private set; }
        public TimeSpan? EndTime { get; private set; }
        public string? Room { get; private set; }

        public string? SchoolYear { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        private Timetable() { }

        public Timetable(Guid classId, Guid subjectId, Guid teacherId, short dayOfWeek, short periodNumber,
                         TimeSpan? startTime = null, TimeSpan? endTime = null, string? room = null, string? schoolYear = null)
        {
            ClassId = classId;
            SubjectId = subjectId;
            TeacherId = teacherId;
            DayOfWeek = dayOfWeek;
            PeriodNumber = periodNumber;
            StartTime = startTime;
            EndTime = endTime;
            Room = room;
            SchoolYear = schoolYear;

            RaiseCreatedEvent();
        }

        public void Update(Timetable updated)
        {
            DayOfWeek = updated.DayOfWeek;
            PeriodNumber = updated.PeriodNumber;
            StartTime = updated.StartTime;
            EndTime = updated.EndTime;
            Room = updated.Room;
            SchoolYear = updated.SchoolYear;
            UpdatedAt = DateTime.UtcNow;

            RaiseUpdatedEvent();
        }

        public void RaiseCreatedEvent()
        {
            AddDomainEvent(new TimetableCreatedEvent(Id, ClassId, SubjectId, TeacherId));
        }

        public void RaiseUpdatedEvent()
        {
            AddDomainEvent(new TimetableUpdatedEvent(Id));
        }

        public void RaiseDeletedEvent()
        {
            AddDomainEvent(new TimetableDeletedEvent(Id));
        }
    }
}