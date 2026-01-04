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
    public class GetExerciseCompletionRateQueryHandler
        : IRequestHandler<GetExerciseCompletionRateQuery, IEnumerable<ExerciseCompletionRateDto>>
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public GetExerciseCompletionRateQueryHandler(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public async Task<IEnumerable<ExerciseCompletionRateDto>> Handle(
            GetExerciseCompletionRateQuery request,
            CancellationToken cancellationToken)
        {
            return await _statisticsRepository.GetExerciseCompletionRateByClassAsync(request.SchoolId);
        }
    }
}
