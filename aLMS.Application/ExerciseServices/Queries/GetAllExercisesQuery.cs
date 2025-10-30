// GetAllExercisesQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.ExerciseServices.Queries
{
    public class GetAllExercisesQuery : IRequest<IEnumerable<ExerciseDto>> { }

    public class GetAllExercisesQueryHandler : IRequestHandler<GetAllExercisesQuery, IEnumerable<ExerciseDto>>
    {
        private readonly IExerciseRepository _repo;
        private readonly IMapper _mapper;

        public GetAllExercisesQueryHandler(IExerciseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExerciseDto>> Handle(GetAllExercisesQuery request, CancellationToken ct)
        {
            var exercises = await _repo.GetAllExercisesAsync();
            return _mapper.Map<IEnumerable<ExerciseDto>>(exercises);
        }
    }
}