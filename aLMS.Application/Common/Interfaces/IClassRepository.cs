using aLMS.Domain.ClassEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task<Class> GetClassByIdAsync(Guid id);
        Task AddClassAsync(Class classEntity);
        Task UpdateClassAsync(Class classEntity);
        Task DeleteClassAsync(Guid id);
        Task<IEnumerable<Class>> GetClassesByGradeIdAsync(Guid gradeId);
        Task<bool> ClassExistsAsync(Guid id);
    }
}