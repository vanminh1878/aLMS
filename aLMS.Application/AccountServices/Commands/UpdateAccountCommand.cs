using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AccountEntity;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.AccountServices.Commands.UpdateAccount
{
    public class UpdateAccountResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
    }

    public class UpdateAccountCommand : IRequest<UpdateAccountResult>
    {
        public UpdateAccountDto Dto { get; set; } = null!;
    }

    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, UpdateAccountResult>
    {
        private readonly IAccountRepository _repo;
        private readonly IMapper _mapper;

        public UpdateAccountCommandHandler(IAccountRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdateAccountResult> Handle(UpdateAccountCommand request, CancellationToken ct)
        {
            try
            {
                var account = await _repo.GetByIdAsync(request.Dto.Id);
                if (account == null)
                    return new UpdateAccountResult { Success = false, Message = "Account not found." };

                var nameExists = await _repo.UsernameExistsAsync(request.Dto.Username, request.Dto.Id);
                if (nameExists)
                    return new UpdateAccountResult { Success = false, Message = "Username already exists." };

                _mapper.Map(request.Dto, account);
                account.RaiseAccountUpdatedEvent();
                await _repo.UpdateAsync(account);

                return new UpdateAccountResult
                {
                    Success = true,
                    Message = "Account updated successfully.",
                    AccountId = account.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateAccountResult
                {
                    Success = false,
                    Message = $"Error updating account: {ex.Message}"
                };
            }
        }
    }
}