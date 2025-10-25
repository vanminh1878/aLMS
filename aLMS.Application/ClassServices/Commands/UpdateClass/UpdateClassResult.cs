using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.UpdateClass
{
    public class UpdateClassResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? ClassId { get; set; }
    }
}
