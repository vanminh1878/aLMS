using aLMS.Domain.StudentQualityEvaluationEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStudentQualityEvaluationRepository
    {
        Task AddAsync(StudentQualityEvaluation entity);
        Task<StudentQualityEvaluation?> GetByIdAsync(Guid id);

    }
}