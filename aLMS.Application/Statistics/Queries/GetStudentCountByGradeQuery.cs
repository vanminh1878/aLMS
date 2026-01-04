using aLMS.Application.Common.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.Statistics.Queries
{
    public class GetStudentCountByGradeQuery : IRequest<IEnumerable<StudentCountByGradeDto>>
    {
        public Guid SchoolId { get; set; }
    }
}
