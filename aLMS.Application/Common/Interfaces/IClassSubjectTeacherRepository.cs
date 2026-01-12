using aLMS.Domain.ClassSubjectEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IClassSubjectTeacherRepository
    {
        Task AddAsync(ClassSubjectTeacher assignment);
        Task<ClassSubjectTeacher?> GetByIdAsync(Guid id);
        Task<ClassSubjectTeacher?> GetByClassSubjectAndTeacherAsync(Guid classSubjectId, Guid teacherId, string? schoolYear);
        Task<IEnumerable<ClassSubjectTeacher>> GetTeachersByClassSubjectAsync(Guid classSubjectId);
        Task<IEnumerable<ClassSubject>> GetClassSubjectsByTeacherAsync(Guid teacherId);
        // Thêm delete nếu cần
    }
}