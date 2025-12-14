using System;

namespace aLMS.Application.Common.Dtos
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool Status { get; set; }
    }

    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public UserDto? User { get; set; }
        public IEnumerable<string>? Permissions { get; set; }
    }
    public class UpdateAccountDto
    {
        public Guid Id { get; set; }
        public string? Username { get; set; } = string.Empty;
        public bool? Status { get; set; }
        public string? Password { get; set; }
    }
}