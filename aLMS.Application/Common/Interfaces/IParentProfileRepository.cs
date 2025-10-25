using aLMS.Domain.ParentProfileEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IParentProfileRepository
    {
        Task<IEnumerable<ParentProfile>> GetAllParentProfilesAsync();
        Task<ParentProfile> GetParentProfileByIdAsync(Guid userId);
        Task AddParentProfileAsync(ParentProfile parentProfile);
        Task UpdateParentProfileAsync(ParentProfile parentProfile);
        Task DeleteParentProfileAsync(Guid userId);
        Task<IEnumerable<ParentProfile>> GetParentProfilesByStudentIdAsync(Guid studentId);
        Task<bool> ParentProfileExistsAsync(Guid userId);
    }
}