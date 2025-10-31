// aLMS.Application.AuthServices.Commands.RefreshToken/RefreshTokenCommand.cs
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Application.Common.Jwt;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.AuthServices.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthResponseDto>
    {
        public RefreshTokenDto Dto { get; set; } = null!;
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(
            IAccountRepository accountRepo,
            IJwtService jwtService,
            IOptions<JwtSettings> jwtSettings)
        {
            _accountRepo = accountRepo;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var account = await _accountRepo.GetByRefreshTokenAsync(request.Dto.RefreshToken);
            if (account == null || _jwtService.IsTokenExpired(account.RefreshTokenExpiry))
            {
                if (account != null) account.ClearRefreshToken();
                return new AuthResponseDto { Success = false, Message = "Invalid or expired refresh token." };
            }

            var principal = _jwtService.ValidateToken(account.RefreshToken!);
            if (principal == null)
                return new AuthResponseDto { Success = false, Message = "Invalid token." };

            var userIdClaim = principal.FindFirst("UserId")?.Value;
            var roleIdClaim = principal.FindFirst("RoleId")?.Value;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim("UserId", userIdClaim ?? ""),
                new Claim("RoleId", roleIdClaim ?? "")
            };

            var permissions = principal.FindAll("Permission").Select(c => c.Value);
            claims.AddRange(permissions.Select(p => new Claim("Permission", p)));

            var newAccessToken = _jwtService.GenerateAccessToken(account, claims);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            account.SetRefreshToken(newRefreshToken, _jwtSettings.RefreshTokenExpiryHours);
            await _accountRepo.UpdateAsync(account);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Token refreshed.",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes)
            };
        }
    }
}