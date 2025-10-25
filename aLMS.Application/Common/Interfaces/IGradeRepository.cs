using aLMS.Domain.GradeEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IGradeRepository
    {
        Task<IEnumerable<Grade>> GetAllGradesAsync();
        Task<Grade> GetGradeByIdAsync(Guid id);
        Task AddGradeAsync(Grade grade);
        Task UpdateGradeAsync(Grade grade);
        Task DeleteGradeAsync(Guid id);
        Task<IEnumerable<Grade>> GetGradesBySchoolIdAsync(Guid schoolId);
        Task<bool> GradeExistsAsync(Guid id);
    }
}