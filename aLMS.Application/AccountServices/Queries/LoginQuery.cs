// aLMS.Application.AccountServices.Queries.Login/LoginQuery.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.AccountServices.Queries.Login
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public Guid? AccountId { get; set; }
    }

    public class LoginQuery : IRequest<LoginResult>
    {
        public LoginDto Dto { get; set; } = null!;
    }

    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResult>
    {
        private readonly IAccountRepository _repo;

        public LoginQueryHandler(IAccountRepository repo) => _repo = repo;

        public async Task<LoginResult> Handle(LoginQuery request, CancellationToken ct)
        {
            try
            {
                var account = await _repo.GetByUsernameAsync(request.Dto.Username);
                if (account == null)
                    return new LoginResult { Success = false, Message = "Invalid username." };

                if (account.Status != true)
                    return new LoginResult { Success = false, Message = "Account disabled." };


                if (!BCrypt.Net.BCrypt.Verify(request.Dto.Password, account.Password))
                    return new LoginResult { Success = false, Message = "Invalid password." };

                var token = "JWT_TOKEN_HERE"; // giả lập
                return new LoginResult
                {
                    Success = true,
                    Message = "Login successful.",
                    Token = token,
                    AccountId = account.Id
                };
            }
            catch (Exception ex)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = $"Error logging in: {ex.Message}"
                };
            }
        }
    }
}