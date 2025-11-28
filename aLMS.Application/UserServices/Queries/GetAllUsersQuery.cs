using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>> { }

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUsersRepository _repo;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUsersRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        var users = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
}

