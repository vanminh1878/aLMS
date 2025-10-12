using aLMS.Domain.ClassEntity;
using aLMS.Domain.Common;
using aLMS.Domain.SchoolEntity;
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

        public Guid ClassId { get; set; }
        public Class Class { get; set; }

        public DateTime EnrollDate { get; set; }
    }
}
