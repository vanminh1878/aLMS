using aLMS.Application.Common.Dtos;
using aLMS.Domain.DepartmentEntity;
using aLMS.Domain.TeacherProfileEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentDto>> GetAllBySchoolIdAsync(Guid schoolId);
        Task<Department?> GetByIdAsync(Guid id);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> NameExistsInSchoolAsync(string name, Guid? excludeId = null);
        
    }
}