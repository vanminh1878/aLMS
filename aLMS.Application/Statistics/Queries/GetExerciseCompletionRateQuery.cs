using aLMS.Application.Common.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.Statistics.Queries
{
    public class GetExerciseCompletionRateQuery : IRequest<IEnumerable<ExerciseCompletionRateDto>>
    {
        public Guid SchoolId { get; set; }
    }
}
