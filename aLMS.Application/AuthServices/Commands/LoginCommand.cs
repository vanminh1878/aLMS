using aLMS.Application.AccountServices.Queries.Login;
using aLMS.Application.Common.Dtos;
using aLMS.Application.Common.Interfaces;
using aLMS.Application.Common.Jwt;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace aLMS.Application.AuthServices.Commands.Login
{
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public LoginDto Dto { get; set; } = null!;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IUsersRepository _usersRepo;
        private readonly IRolePermissionRepository _rpRepo;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public LoginCommandHandler(
            IAccountRepository accountRepo,
            IUsersRepository usersRepo,
            IRolePermissionRepository rpRepo,
            IJwtService jwtService,
            IOptions<JwtSettings> jwtSettings)
        {
            _accountRepo = accountRepo;
            _usersRepo = usersRepo;
            _rpRepo = rpRepo;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken ct)
        {
            var account = await _accountRepo.GetByUsernameAsync(request.Dto.Username);
            if (account == null)
                return new AuthResponseDto { Success = false, Message = "Invalid username." };

            if (account.Status != true)
                return new AuthResponseDto { Success = false, Message = "Account disabled." };

            if (!BCrypt.Net.BCrypt.Verify(request.Dto.Password, account.Password))
                return new AuthResponseDto { Success = false, Message = "Invalid password." };

            var user = await _usersRepo.GetByAccountIdAsync(account.Id);
            var permissions = await _rpRepo.GetByRoleIdAsync(user?.RoleId ?? Guid.Empty);
            var permissionNames = permissions.Select(p => p.Permission.PermissionName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim("UserId", user?.Id.ToString() ?? ""),
                new Claim("RoleId", user?.RoleId?.ToString() ?? "")
            };
            claims.AddRange(permissionNames.Select(p => new Claim("Permission", p)));

            var accessToken = _jwtService.GenerateAccessToken(account, claims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            account.SetRefreshToken(refreshToken, _jwtSettings.RefreshTokenExpiryHours);
            await _accountRepo.UpdateAsync(account);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login successful.",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                User = user == null ? null : new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    RoleId = user.RoleId,
                },
                Permissions = permissionNames
            };
        }
    }
}