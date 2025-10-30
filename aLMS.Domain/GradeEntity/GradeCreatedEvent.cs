using aLMS.Domain.Common;
using System;

namespace aLMS.Domain.GradeEntity
{
    public class GradeCreatedEvent : IDomainEvent
    {
        public Guid GradeId { get; }
        public string GradeName { get; }
        public string SchoolYear { get; }
        public Guid SchoolId { get; }

        public GradeCreatedEvent(Guid gradeId, string gradeName, string schoolYear, Guid schoolId)
        {
            GradeId = gradeId;
            GradeName = gradeName;
            SchoolYear = schoolYear;
            SchoolId = schoolId;
        }
    }

    public class GradeUpdatedEvent : IDomainEvent
    {
        public Guid GradeId { get; }
        public string GradeName { get; }
        public string SchoolYear { get; }
        public Guid SchoolId { get; }

        public GradeUpdatedEvent(Guid gradeId, string gradeName, string schoolYear, Guid schoolId)
        {
            GradeId = gradeId;
            GradeName = gradeName;
            SchoolYear = schoolYear;
            SchoolId = schoolId;
        }
    }

    public class GradeDeletedEvent : IDomainEvent
    {
        public Guid GradeId { get; }

        public GradeDeletedEvent(Guid gradeId)
        {
            GradeId = gradeId;
        }
    }
}