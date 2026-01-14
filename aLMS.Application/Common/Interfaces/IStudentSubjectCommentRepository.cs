using aLMS.Domain.StudentEvaluationEntity;
using aLMS.Domain.StudentSubjectCommentEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStudentSubjectCommentRepository
    {
        Task AddAsync(StudentSubjectComment entity);
        Task<StudentSubjectComment?> GetByIdAsync(Guid id);
    }
}