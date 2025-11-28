using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

public class GetAllRolesQuery : IRequest<IEnumerable<RoleDto>> { }

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IRoleRepository _repo;
    private readonly IMapper _mapper;

    public GetAllRolesQueryHandler(IRoleRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken ct)
    {
        var roles = await _repo.GetAllRolesAsync();
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }
}

// GetRoleByIdQuery.cs
public class GetRoleByIdQuery : IRequest<RoleDto>
{
    public Guid Id { get; set; }
}

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IRoleRepository _repo;
    private readonly IMapper _mapper;

    public GetRoleByIdQueryHandler(IRoleRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken ct)
    {
        var role = await _repo.GetRoleByIdAsync(request.Id);
        return role == null ? null : _mapper.Map<RoleDto>(role);
    }
}