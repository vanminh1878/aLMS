using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

public class GetAllPermissionsQuery : IRequest<IEnumerable<PermissionDto>> { }

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IPermissionRepository _repo;
    private readonly IMapper _mapper;

    public GetAllPermissionsQueryHandler(IPermissionRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken ct)
    {
        var permissions = await _repo.GetAllPermissionsAsync();
        return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
    }
}

// GetPermissionByIdQuery.cs
public class GetPermissionByIdQuery : IRequest<PermissionDto>
{
    public Guid Id { get; set; }
}

public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto>
{
    private readonly IPermissionRepository _repo;
    private readonly IMapper _mapper;

    public GetPermissionByIdQueryHandler(IPermissionRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PermissionDto> Handle(GetPermissionByIdQuery request, CancellationToken ct)
    {
        var permission = await _repo.GetPermissionByIdAsync(request.Id);
        return permission == null ? null : _mapper.Map<PermissionDto>(permission);
    }
}