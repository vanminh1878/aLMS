using aLMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.ClassEntity
{
    public class ClassCreatedEvent : IDomainEvent
    {
        public Guid ClassId { get; }
        public string ClassName { get; }
        public Guid GradeId { get; }

        public ClassCreatedEvent(Guid classId, string className, Guid gradeId)
        {
            ClassId = classId;
            ClassName = className;
            GradeId = gradeId;
        }
    }

    public class ClassUpdatedEvent : IDomainEvent
    {
        public Guid ClassId { get; }
        public string ClassName { get; }
        public Guid GradeId { get; }

        public ClassUpdatedEvent(Guid classId, string className, Guid gradeId)
        {
            ClassId = classId;
            ClassName = className;
            GradeId = gradeId;
        }
    }

    public class ClassDeletedEvent : IDomainEvent
    {
        public Guid ClassId { get; }

        public ClassDeletedEvent(Guid classId)
        {
            ClassId = classId;
        }
    }
}
