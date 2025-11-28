using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.RoleServices.Commands.DeleteRole
{
    public class DeleteRoleResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? RoleId { get; set; }
    }

    public class DeleteRoleCommand : IRequest<DeleteRoleResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, DeleteRoleResult>
    {
        private readonly IRoleRepository _repo;

        public DeleteRoleCommandHandler(IRoleRepository repo) => _repo = repo;

        public async Task<DeleteRoleResult> Handle(DeleteRoleCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.RoleExistsAsync(request.Id);
                if (!exists)
                    return new DeleteRoleResult { Success = false, Message = "Role not found.", RoleId = request.Id };

                await _repo.DeleteRoleAsync(request.Id);
                return new DeleteRoleResult
                {
                    Success = true,
                    Message = "Role deleted successfully.",
                    RoleId = request.Id
                };
            }
            catch (Exception ex)
            {
                return new DeleteRoleResult
                {
                    Success = false,
                    Message = $"Error deleting role: {ex.Message}",
                    RoleId = request.Id
                };
            }
        }
    }
}