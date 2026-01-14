using aLMS.Domain.NotificationEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification entity);
        Task UpdateAsync(Notification entity);
        Task DeleteAsync(Guid id);
        Task<Notification?> GetByIdAsync(Guid id);
        Task<IEnumerable<Notification>> GetByClassIdAsync(Guid classId);
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
    }
}