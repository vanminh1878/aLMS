using aLMS.Domain.AccountEntity;
using aLMS.Domain.BehaviourEntity;
using aLMS.Domain.Common;
using aLMS.Domain.ParentProfileEntity;
using aLMS.Domain.RoleEntity;
using aLMS.Domain.SchoolEntity;
using aLMS.Domain.StudentExerciseEntity;
using aLMS.Domain.StudentProfileEntity;
using aLMS.Domain.TeacherProfileEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.UserEntity
{
    public class User : Entity
    {
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Guid? SchoolId { get; set; }
        public SchoolEntity.School School { get; set; }

        public Guid? AccountId { get; set; }
        public Account Account { get; set; }

        public Guid? RoleId { get; set; }
        public Role Role { get; set; }

        public StudentProfile StudentProfile { get; set; }
        public TeacherProfile TeacherProfile { get; set; }
        public ParentProfile ParentProfile { get; set; }

        public ICollection<StudentExercise> StudentExercises { get; set; }
        public ICollection<Behaviour> Behaviours { get; set; }
    }
}
