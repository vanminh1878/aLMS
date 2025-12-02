using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Application.Common.Jwt;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.AccountServices.Queries.Login
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public Guid? AccountId { get; set; }
    }

    public class LoginQuery : IRequest<LoginResult>
    {
        public LoginDto Dto { get; set; } = null!;
    }

    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResult>
    {
        private readonly IAccountRepository _repo;

        private readonly JwtSettings _jwtSettings;
        private readonly IJwtService _jwtService;
        public LoginQueryHandler(IAccountRepository repo, IJwtService jwtService, IOptions<JwtSettings> jwtSettings)
        {
            _repo = repo;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
        }

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

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new Claim(ClaimTypes.Name, account.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var accessToken = _jwtService.GenerateAccessToken(account, claims);
                var refreshToken = _jwtService.GenerateRefreshToken();
                return new LoginResult
                {
                    Success = true,
                    Message = "Login successful.",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
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