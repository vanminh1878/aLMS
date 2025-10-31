// aLMS.Application.Common.Models/JwtSettings.cs
namespace aLMS.Application.Common.Jwt
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public int AccessTokenExpiryMinutes { get; set; } = 15;
        public int RefreshTokenExpiryHours { get; set; } = 24;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}