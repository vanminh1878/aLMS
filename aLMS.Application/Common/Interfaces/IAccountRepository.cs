using aLMS.Domain.AccountEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByUsernameAsync(string username);
        Task<Account?> GetByIdAsync(Guid id);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(Guid id);
        Task<bool> UsernameExistsAsync(string username, Guid? excludeId = null);
    }
}