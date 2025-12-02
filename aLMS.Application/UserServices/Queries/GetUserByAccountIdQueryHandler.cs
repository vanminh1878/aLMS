using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.UserServices.Queries
{
    public class GetUserByAccountIdQueryHandler : IRequestHandler<GetUserByAccountIdQuery, UserDto?>
    {
        private readonly IUsersRepository _repo;
        private readonly IMapper _mapper;

        public GetUserByAccountIdQueryHandler(IUsersRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserDto?> Handle(GetUserByAccountIdQuery request, CancellationToken ct)
        {
            var user = await _repo.GetByAccountIdAsync(request.AccountId);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }
    }
}
