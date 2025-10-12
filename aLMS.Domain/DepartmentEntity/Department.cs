using aLMS.Domain.Common;
using aLMS.Domain.TeacherProfileEntity;
using aLMS.Domain.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.DepartmentEntity
{
    public class Department : Entity
    {
        public string DepartmentName { get; set; }
        public Guid HeadId { get; set; }
        public User Head { get; set; }

        public ICollection<TeacherProfile> TeacherProfiles { get; set; }
    }
}
