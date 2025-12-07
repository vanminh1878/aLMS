using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.AddStudentsToClass
{

        public class AddStudentToClassDto
        {
            public string StudentName { get; set; } = string.Empty;
            public DateTime StudentDateOfBirth { get; set; }
            public DateTime StudentEnrollDate { get; set; }
            public string Gender { get; set; } 

            public string ParentName { get; set; } = string.Empty;
            public string ParentPhone { get; set; } = string.Empty;
            public DateTime ParentDateOfBirth { get; set; }
            public string ParentGender { get; set; }
            public string ParentEmail { get; set; }
            public string Address { get; set; }
            
            public Guid SchoolId { get; set; }
    }
    
}
