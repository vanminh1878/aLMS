using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

public class GetExercisesByTopicIdQuery : IRequest<IEnumerable<ExerciseDto>>
{
    public Guid TopicId { get; set; }
}

public class GetExercisesByLessonIdQueryHandler : IRequestHandler<GetExercisesByTopicIdQuery, IEnumerable<ExerciseDto>>
{
    private readonly IExerciseRepository _repo;
    private readonly IMapper _mapper;

    public GetExercisesByLessonIdQueryHandler(IExerciseRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ExerciseDto>> Handle(GetExercisesByTopicIdQuery request, CancellationToken ct)
    {
        var exercises = await _repo.GetExercisesByTopicIdAsync(request.TopicId);
        return _mapper.Map<IEnumerable<ExerciseDto>>(exercises);
    }
}