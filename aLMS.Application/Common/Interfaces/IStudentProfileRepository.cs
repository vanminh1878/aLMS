using aLMS.Domain.StudentProfileEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStudentProfileRepository
    {
        Task<StudentProfile?> GetByUserIdAsync(Guid userId);
        Task AddAsync(StudentProfile profile);
        Task UpdateAsync(StudentProfile profile);
        Task DeleteAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId);
    }
}