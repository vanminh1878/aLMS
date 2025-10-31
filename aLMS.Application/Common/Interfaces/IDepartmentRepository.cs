using aLMS.Domain.DepartmentEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllBySchoolIdAsync(Guid schoolId);
        Task<Department?> GetByIdAsync(Guid id);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> NameExistsInSchoolAsync(string name, Guid schoolId, Guid? excludeId = null);
    }
}