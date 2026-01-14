using aLMS.Domain.StudentEvaluationEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStudentEvaluationRepository
    {
        Task AddAsync(StudentEvaluation entity);
        Task UpdateAsync(StudentEvaluation entity);
        Task<StudentEvaluation?> GetByIdAsync(Guid id);
        Task<IEnumerable<StudentEvaluation>> GetByStudentIdAsync(Guid studentId, string? semester, string? schoolYear);
        Task<IEnumerable<StudentEvaluation>> GetByClassIdAsync(Guid classId, string? semester, string? schoolYear);
    }
}