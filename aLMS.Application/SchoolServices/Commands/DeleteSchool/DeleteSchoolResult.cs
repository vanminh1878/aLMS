using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Commands.DeleteSchool
{
    public class DeleteSchoolResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? DeletedSchoolId { get; set; }
    }
}
