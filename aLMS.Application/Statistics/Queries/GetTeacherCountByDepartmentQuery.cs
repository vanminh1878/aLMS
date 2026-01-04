using aLMS.Application.Common.DTOs;
using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.Statistics.Queries
{
    public class GetTeacherCountByDepartmentQuery : IRequest<IEnumerable<TeacherCountByDepartmentDto>>
    {
        public Guid SchoolId { get; set; }
    }
}
