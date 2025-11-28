using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.UserServices.Commands.DeleteUser
{
    public class DeleteUserResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }

    public class DeleteUserCommand : IRequest<DeleteUserResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResult>
    {
        private readonly IUsersRepository _repo;

        public DeleteUserCommandHandler(IUsersRepository repo) => _repo = repo;

        public async Task<DeleteUserResult> Handle(DeleteUserCommand request, CancellationToken ct)
        {
            try
            {
                var user = await _repo.GetByIdAsync(request.Id);
                if (user == null)
                    return new DeleteUserResult { Success = false, Message = "User not found.", UserId = request.Id };

                await _repo.DeleteAsync(request.Id);
                return new DeleteUserResult
                {
                    Success = true,
                    Message = "User deleted successfully.",
                    UserId = request.Id
                };
            }
            catch (Exception ex)
            {
                return new DeleteUserResult
                {
                    Success = false,
                    Message = $"Error deleting user: {ex.Message}",
                    UserId = request.Id
                };
            }
        }
    }
}