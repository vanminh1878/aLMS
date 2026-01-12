using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace aLMS.Application.Common.Dtos
{
    public class AddSubjectToClassDto
    {
        public Guid SubjectId { get; set; }
        public string? SchoolYear { get; set; }
    }

    public class AddSubjectToClassResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? ClassSubjectId { get; set; }
    }
}
