using aLMS.Domain.UserEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByAccountIdAsync(Guid accountId);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null);
    }
}