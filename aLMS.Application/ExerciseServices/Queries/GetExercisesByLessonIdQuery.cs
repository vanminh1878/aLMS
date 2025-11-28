using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

public class GetExercisesByLessonIdQuery : IRequest<IEnumerable<ExerciseDto>>
{
    public Guid LessonId { get; set; }
}

public class GetExercisesByLessonIdQueryHandler : IRequestHandler<GetExercisesByLessonIdQuery, IEnumerable<ExerciseDto>>
{
    private readonly IExerciseRepository _repo;
    private readonly IMapper _mapper;

    public GetExercisesByLessonIdQueryHandler(IExerciseRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ExerciseDto>> Handle(GetExercisesByLessonIdQuery request, CancellationToken ct)
    {
        var exercises = await _repo.GetExercisesByLessonIdAsync(request.LessonId);
        return _mapper.Map<IEnumerable<ExerciseDto>>(exercises);
    }
}