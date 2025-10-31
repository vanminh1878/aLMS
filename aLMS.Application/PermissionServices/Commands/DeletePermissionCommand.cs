// aLMS.Application.PermissionServices.Commands.DeletePermission/DeletePermissionCommand.cs
using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.PermissionServices.Commands.DeletePermission
{
    public class DeletePermissionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? PermissionId { get; set; }
    }

    public class DeletePermissionCommand : IRequest<DeletePermissionResult>
    {
        public Guid Id { get; set; }
    }

    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, DeletePermissionResult>
    {
        private readonly IPermissionRepository _repo;

        public DeletePermissionCommandHandler(IPermissionRepository repo) => _repo = repo;

        public async Task<DeletePermissionResult> Handle(DeletePermissionCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.PermissionExistsAsync(request.Id);
                if (!exists)
                    return new DeletePermissionResult { Success = false, Message = "Permission not found.", PermissionId = request.Id };

                await _repo.DeletePermissionAsync(request.Id);
                return new DeletePermissionResult
                {
                    Success = true,
                    Message = "Permission deleted successfully.",
                    PermissionId = request.Id
                };
            }
            catch (Exception ex)
            {
                return new DeletePermissionResult
                {
                    Success = false,
                    Message = $"Error deleting permission: {ex.Message}",
                    PermissionId = request.Id
                };
            }
        }
    }
}