using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

public class GetExerciseByIdQuery : IRequest<ExerciseDto>
{
    public Guid Id { get; set; }
}

public class GetExerciseByIdQueryHandler : IRequestHandler<GetExerciseByIdQuery, ExerciseDto>
{
    private readonly IExerciseRepository _repo;
    private readonly IMapper _mapper;

    public GetExerciseByIdQueryHandler(IExerciseRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ExerciseDto> Handle(GetExerciseByIdQuery request, CancellationToken ct)
    {
        var exercise = await _repo.GetExerciseByIdAsync(request.Id);
        return exercise == null ? null : _mapper.Map<ExerciseDto>(exercise);
    }
}