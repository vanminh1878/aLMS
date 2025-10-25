using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.SchoolServices.Commands.UpdateSchool
{
    public class UpdateSchoolResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? SchoolId { get; set; }
    }

}
