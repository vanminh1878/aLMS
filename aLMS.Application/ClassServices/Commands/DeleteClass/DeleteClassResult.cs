using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.ClassServices.Commands.DeleteClass
{
    public class DeleteClassResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
