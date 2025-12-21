using aLMS.Application.Common.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.StudentExerciseServices.Queries
{
    public class GetClassExerciseOverviewQuery : IRequest<ClassExerciseOverviewDto>
    {
        public Guid ExerciseId { get; set; }
        public Guid ClassId { get; set; }
    }
}
