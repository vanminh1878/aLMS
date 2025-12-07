using aLMS.Application.Common.Interfaces;
using MediatR;
using System;

namespace aLMS.Application.RoleServices.Queries
{
    public class GetRoleIdByNameQuery : IRequest<Guid?>
    {
        public string RoleName { get; set; } = string.Empty;
    }

    public class GetRoleIdByNameQueryHandler : IRequestHandler<GetRoleIdByNameQuery, Guid?>
    {
        private readonly IRoleRepository _roleRepo;

        public GetRoleIdByNameQueryHandler(IRoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task<Guid?> Handle(GetRoleIdByNameQuery request, CancellationToken ct)
        {
            var roleName = request.RoleName.Trim();
            if (string.IsNullOrEmpty(roleName))
                return null;

            return await _roleRepo.GetRoleIdByNameAsync(roleName);
        }
    }
}