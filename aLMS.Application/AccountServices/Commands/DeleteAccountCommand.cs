using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.AccountServices.Commands.DeleteAccount
{
    public class DeleteAccountResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
    }

    public class DeleteAccountCommand : IRequest<DeleteAccountResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, DeleteAccountResult>
    {
        private readonly IAccountRepository _repo;

        public DeleteAccountCommandHandler(IAccountRepository repo) => _repo = repo;

        public async Task<DeleteAccountResult> Handle(DeleteAccountCommand request, CancellationToken ct)
        {
            try
            {
                var account = await _repo.GetByIdAsync(request.Id);
                if (account == null)
                    return new DeleteAccountResult { Success = false, Message = "Account not found.", AccountId = request.Id };

                await _repo.DeleteAsync(request.Id);
                return new DeleteAccountResult
                {
                    Success = true,
                    Message = "Account deleted successfully.",
                    AccountId = request.Id
                };
            }
            catch (Exception ex)
            {
                return new DeleteAccountResult
                {
                    Success = false,
                    Message = $"Error deleting account: {ex.Message}",
                    AccountId = request.Id
                };
            }
        }
    }
}