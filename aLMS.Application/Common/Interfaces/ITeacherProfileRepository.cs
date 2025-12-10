using aLMS.Domain.TeacherProfileEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface ITeacherProfileRepository
    {
        Task<TeacherProfile?> GetByUserIdAsync(Guid userId);
        Task AddAsync(TeacherProfile profile);
        Task UpdateAsync(TeacherProfile profile);
        Task DeleteAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId);
        Task<List<TeacherProfile>> GetBySchoolIdAsync(Guid schoolId);
        Task<List<TeacherProfile>> GetByDepartmentIdAsync(Guid departmentId);
    }
}