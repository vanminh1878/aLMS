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
    public class GetTeacherCountByDepartmentQueryHandler
        : IRequestHandler<GetTeacherCountByDepartmentQuery, IEnumerable<TeacherCountByDepartmentDto>>
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public GetTeacherCountByDepartmentQueryHandler(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public async Task<IEnumerable<TeacherCountByDepartmentDto>> Handle(
            GetTeacherCountByDepartmentQuery request,
            CancellationToken cancellationToken)
        {
            return await _statisticsRepository.GetTeacherCountByDepartmentAsync(request.SchoolId);
        }
    }
}
