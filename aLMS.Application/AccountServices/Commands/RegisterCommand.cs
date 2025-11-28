using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Domain.AccountEntity;
using AutoMapper;
using MediatR;
using BCrypt.Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.AccountServices.Commands.Register
{
    public class RegisterResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
    }

    public class RegisterCommand : IRequest<RegisterResult>
    {
        public RegisterDto Dto { get; set; } = null!;
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IAccountRepository _repo;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IAccountRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken ct)
        {
            try
            {
                var exists = await _repo.UsernameExistsAsync(request.Dto.Username);
                if (exists)
                    return new RegisterResult { Success = false, Message = "Username already exists." };

                var account = _mapper.Map<Account>(request.Dto);
                account.Id = Guid.NewGuid();
                account.Password = BCrypt.Net.BCrypt.HashPassword(request.Dto.Password); // hash
                account.RaiseAccountCreatedEvent();
                await _repo.AddAsync(account);

                return new RegisterResult
                {
                    Success = true,
                    Message = "Account registered successfully.",
                    AccountId = account.Id
                };
            }
            catch (Exception ex)
            {
                return new RegisterResult
                {
                    Success = false,
                    Message = $"Error registering: {ex.Message}"
                };
            }
        }
    }
}