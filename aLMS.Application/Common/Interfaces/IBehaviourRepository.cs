using aLMS.Domain.BehaviourEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IBehaviourRepository
    {
        Task<IEnumerable<Behaviour>> GetByStudentIdAsync(Guid studentId);
        Task<Behaviour?> GetByIdAsync(Guid id);
        Task AddAsync(Behaviour behaviour);
        Task UpdateAsync(Behaviour behaviour);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<int> GetNextOrderAsync(Guid studentId);
    }
}