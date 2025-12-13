using aLMS.Domain.ClassEntity;
using aLMS.Domain.Common;
using aLMS.Domain.DepartmentEntity;
using aLMS.Domain.SchoolEntity;
using aLMS.Domain.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.TeacherProfileEntity
{
    public class TeacherProfile : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public DateTime HireDate { get; set; }
        public string Specialization { get; set; }
        public Class? HomeroomClass { get; set; }
        public void RaiseTeacherProfileCreatedEvent() => AddDomainEvent(new TeacherProfileCreatedEvent(UserId, DepartmentId));
        public void RaiseTeacherProfileUpdatedEvent() => AddDomainEvent(new TeacherProfileUpdatedEvent(UserId, DepartmentId));
        public void RaiseTeacherProfileDeletedEvent() => AddDomainEvent(new TeacherProfileDeletedEvent(UserId));

    }
}
