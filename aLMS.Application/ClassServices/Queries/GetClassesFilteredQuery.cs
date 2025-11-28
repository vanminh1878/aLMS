using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

public class GetClassesFilteredQuery : IRequest<IEnumerable<ClassDto>>
{
    public string? Grade { get; set; }
    public string? SchoolYear { get; set; }
}

public class GetClassesFilteredQueryHandler : IRequestHandler<GetClassesFilteredQuery, IEnumerable<ClassDto>>
{
    private readonly IClassRepository _repo;
    private readonly IMapper _mapper;

    public GetClassesFilteredQueryHandler(IClassRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClassDto>> Handle(GetClassesFilteredQuery request, CancellationToken ct)
    {
        var classes = await _repo.GetClassesFilteredAsync(request.Grade, request.SchoolYear);
        return _mapper.Map<IEnumerable<ClassDto>>(classes);
    }
}