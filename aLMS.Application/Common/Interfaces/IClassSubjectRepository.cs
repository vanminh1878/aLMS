using aLMS.Domain.ClassSubjectEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IClassSubjectRepository
    {
        Task AddClassSubjectAsync(ClassSubject classSubject);
        Task<ClassSubject?> GetClassSubjectByClassAndSubjectAsync(Guid classId, Guid subjectId, string? schoolYear);
        Task<ClassSubject?> GetClassSubjectByIdAsync(Guid id);
        Task<IEnumerable<ClassSubject>> GetAllClassSubjectsAsync();
        Task<IEnumerable<ClassSubject>> GetClassSubjectsByClassIdAsync(Guid classId);
        Task<IEnumerable<ClassSubject>> GetClassSubjectsBySubjectIdAsync(Guid subjectId);
        Task UpdateClassSubjectAsync(ClassSubject classSubject);
        Task DeleteClassSubjectAsync(Guid id);
    }
}