using aLMS.Domain.ClassEntity;
using aLMS.Domain.StudentProfileEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.StudentClassEnrollmentEntity
{
    public class StudentClassEnrollment
    {
        public Guid StudentProfileId { get; set; }
        public Guid ClassId { get; set; }           
        public StudentProfile StudentProfile { get; set; } = null!;
        public Class Class { get; set; } = null!;
    }
}
