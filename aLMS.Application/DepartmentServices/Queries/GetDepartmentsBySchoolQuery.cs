// GetDepartmentsBySchoolQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

public class GetDepartmentsBySchoolQuery : IRequest<IEnumerable<DepartmentDto>>
{
    public Guid SchoolId { get; set; }
}

public class GetDepartmentByIdQuery : IRequest<DepartmentDto>
{
    public Guid Id { get; set; }
}

public class DepartmentQueryHandler :
    IRequestHandler<GetDepartmentsBySchoolQuery, IEnumerable<DepartmentDto>>,
    IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
{
    private readonly IDepartmentRepository _repo;
    private readonly IMapper _mapper;

    public DepartmentQueryHandler(IDepartmentRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentDto>> Handle(GetDepartmentsBySchoolQuery request, CancellationToken ct)
    {
        var departments = await _repo.GetAllBySchoolIdAsync(request.SchoolId);
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery request, CancellationToken ct)
    {
        var department = await _repo.GetByIdAsync(request.Id);
        return department == null ? null : _mapper.Map<DepartmentDto>(department);
    }
}