using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.Common.DTOs
{
    public class AssignedSubjectDto
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty; 
        public string SchoolYear { get; set; } = string.Empty;
    }
}
