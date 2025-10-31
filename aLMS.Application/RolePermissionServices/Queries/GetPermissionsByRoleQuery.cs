// aLMS.Application.RolePermissionServices.Queries/GetPermissionsByRoleQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.RolePermissionServices.Queries
{
    public class GetPermissionsByRoleQuery : IRequest<IEnumerable<RolePermissionDto>>
    {
        public Guid RoleId { get; set; }
    }

    public class GetPermissionsByRoleQueryHandler : IRequestHandler<GetPermissionsByRoleQuery, IEnumerable<RolePermissionDto>>
    {
        private readonly IRolePermissionRepository _rpRepo;
        private readonly IMapper _mapper;

        public GetPermissionsByRoleQueryHandler(IRolePermissionRepository rpRepo, IMapper mapper)
        {
            _rpRepo = rpRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RolePermissionDto>> Handle(GetPermissionsByRoleQuery request, CancellationToken ct)
        {
            var rps = await _rpRepo.GetByRoleIdAsync(request.RoleId);
            return _mapper.Map<IEnumerable<RolePermissionDto>>(rps);
        }
    }
}