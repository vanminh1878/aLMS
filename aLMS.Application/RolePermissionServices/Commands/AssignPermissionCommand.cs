// aLMS.Application.RolePermissionServices.Commands.AssignPermission/AssignPermissionCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.RolePermissionEntity;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.RolePermissionServices.Commands.AssignPermission
{
    public class AssignPermissionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class AssignPermissionCommand : IRequest<AssignPermissionResult>
    {
        public AssignPermissionDto Dto { get; set; } = null!;
    }

    public class AssignPermissionCommandHandler : IRequestHandler<AssignPermissionCommand, AssignPermissionResult>
    {
        private readonly IRolePermissionRepository _rpRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IPermissionRepository _permRepo;

        public AssignPermissionCommandHandler(
            IRolePermissionRepository rpRepo,
            IRoleRepository roleRepo,
            IPermissionRepository permRepo)
        {
            _rpRepo = rpRepo;
            _roleRepo = roleRepo;
            _permRepo = permRepo;
        }

        public async Task<AssignPermissionResult> Handle(AssignPermissionCommand request, CancellationToken ct)
        {
            try
            {
                var roleExists = await _roleRepo.RoleExistsAsync(request.Dto.RoleId);
                if (!roleExists)
                    return new AssignPermissionResult { Success = false, Message = "Role not found." };

                var permExists = await _permRepo.PermissionExistsAsync(request.Dto.PermissionId);
                if (!permExists)
                    return new AssignPermissionResult { Success = false, Message = "Permission not found." };

                var exists = await _rpRepo.ExistsAsync(request.Dto.RoleId, request.Dto.PermissionId);
                if (exists)
                    return new AssignPermissionResult { Success = false, Message = "Permission already assigned." };

                var rp = new RolePermission
                {
                    Id = Guid.NewGuid(),
                    RoleId = request.Dto.RoleId,
                    PermissionId = request.Dto.PermissionId
                };
                rp.RaiseRolePermissionAddedEvent();
                await _rpRepo.AddAsync(rp);

                return new AssignPermissionResult
                {
                    Success = true,
                    Message = "Permission assigned successfully."
                };
            }
            catch (Exception ex)
            {
                return new AssignPermissionResult
                {
                    Success = false,
                    Message = $"Error assigning permission: {ex.Message}"
                };
            }
        }
    }
}