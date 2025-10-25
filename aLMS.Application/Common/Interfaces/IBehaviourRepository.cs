using aLMS.Domain.BehaviourEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IBehaviourRepository
    {
        Task<IEnumerable<Behaviour>> GetAllBehavioursAsync();
        Task<Behaviour> GetBehaviourByIdAsync(Guid id);
        Task AddBehaviourAsync(Behaviour behaviour);
        Task UpdateBehaviourAsync(Behaviour behaviour);
        Task DeleteBehaviourAsync(Guid id);
        Task<IEnumerable<Behaviour>> GetBehavioursByStudentIdAsync(Guid studentId);
        Task<bool> BehaviourExistsAsync(Guid id);
    }
}