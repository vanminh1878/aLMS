// aLMS.Application.RolePermissionServices.Commands.RemovePermission/RemovePermissionCommand.cs
using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.RolePermissionServices.Commands.RemovePermission
{
    public class RemovePermissionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class RemovePermissionCommand : IRequest<RemovePermissionResult>
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }

    public class RemovePermissionCommandHandler : IRequestHandler<RemovePermissionCommand, RemovePermissionResult>
    {
        private readonly IRolePermissionRepository _rpRepo;

        public RemovePermissionCommandHandler(IRolePermissionRepository rpRepo) => _rpRepo = rpRepo;

        public async Task<RemovePermissionResult> Handle(RemovePermissionCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _rpRepo.ExistsAsync(request.RoleId, request.PermissionId);
                if (!exists)
                    return new RemovePermissionResult { Success = false, Message = "Permission not assigned to this role." };

                await _rpRepo.DeleteAsync(request.RoleId, request.PermissionId);
                return new RemovePermissionResult
                {
                    Success = true,
                    Message = "Permission removed successfully."
                };
            }
            catch (Exception ex)
            {
                return new RemovePermissionResult
                {
                    Success = false,
                    Message = $"Error removing permission: {ex.Message}"
                };
            }
        }
    }
}