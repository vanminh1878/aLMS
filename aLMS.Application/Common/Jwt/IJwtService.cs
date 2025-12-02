using aLMS.Domain.AccountEntity;
using System.Security.Claims;

namespace aLMS.Application.Common.Jwt
{
    public interface IJwtService
    {
        string GenerateAccessToken(Account account, IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string token);
        bool IsTokenExpired(DateTime? expiry);
    }
}