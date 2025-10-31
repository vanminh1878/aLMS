// aLMS.Application.Common.Dtos/AccountDtos.cs
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

    public class UpdateAccountDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}