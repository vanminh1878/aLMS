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
    public class GetStudentCountByGradeQueryHandler
        : IRequestHandler<GetStudentCountByGradeQuery, IEnumerable<StudentCountByGradeDto>>
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public GetStudentCountByGradeQueryHandler(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public async Task<IEnumerable<StudentCountByGradeDto>> Handle(
            GetStudentCountByGradeQuery request,
            CancellationToken cancellationToken)
        {
            return await _statisticsRepository.GetStudentCountByGradeAsync(request.SchoolId);
        }
    }
}
