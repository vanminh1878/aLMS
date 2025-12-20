using aLMS.Domain.ClassEntity;
using aLMS.Domain.Common;
using aLMS.Domain.SchoolEntity;
using aLMS.Domain.StudentClassEnrollmentEntity;
using aLMS.Domain.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.StudentProfileEntity
{
    public class StudentProfile : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid SchoolId { get; set; }
        public School School { get; set; }
        public DateTime EnrollDate { get; set; }
        public ICollection<StudentClassEnrollment> ClassEnrollments { get; set; } = new List<StudentClassEnrollment>();
        public void RaiseStudentProfileCreatedEvent() => AddDomainEvent(new StudentProfileCreatedEvent(UserId, SchoolId));
        public void RaiseStudentProfileUpdatedEvent() => AddDomainEvent(new StudentProfileUpdatedEvent(UserId, SchoolId));
        public void RaiseStudentProfileDeletedEvent() => AddDomainEvent(new StudentProfileDeletedEvent(UserId));

    }
}
