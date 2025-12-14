using aLMS.Application.Common.Dtos;
using aLMS.Domain.ParentProfileEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IParentProfileRepository
    {
        Task<ParentProfile?> GetByParentIdAsync(Guid parentId);
        Task<IEnumerable<ParentProfileDto>> GetByStudentIdAsync(Guid studentId);
        Task AddAsync(ParentProfile profile);
        Task UpdateAsync(ParentProfile profile);
        Task DeleteAsync(Guid parentId, Guid studentId);
        Task<bool> ExistsAsync(Guid parentId, Guid studentId);
    }
}