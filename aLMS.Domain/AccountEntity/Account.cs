using aLMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.AccountEntity
{
    public class Account: Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? Status { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public void SetRefreshToken(string token, int expiryHours)
        {
            RefreshToken = token;
            RefreshTokenExpiry = DateTime.UtcNow.AddHours(expiryHours);
        }

        public void ClearRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpiry = null;
        }
        public void RaiseAccountCreatedEvent() => AddDomainEvent(new AccountCreatedEvent(Id, Username));
        public void RaiseAccountUpdatedEvent() => AddDomainEvent(new AccountUpdatedEvent(Id, Username));
        public void RaiseAccountDeletedEvent() => AddDomainEvent(new AccountDeletedEvent(Id));
        public void RaiseAccountDisabledEvent() => AddDomainEvent(new AccountDisabledEvent(Id));
    }
}
