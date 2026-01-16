using aLMS.Application.Common.DTOs;
using aLMS.Domain.SubjectEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();
        Task<Subject> GetSubjectByIdAsync(Guid id);
        Task AddSubjectAsync(Subject subject);
        Task UpdateSubjectAsync(Subject subject);
        Task DeleteSubjectAsync(Guid id);
        Task<bool> SubjectExistsAsync(Guid id);
        Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(Guid classId);
        Task<Subject> GetSubjectByTopicIdAsync(Guid id);
        Task<List<AssignedSubjectDto>> GetAssignedSubjectsByTeacherAsync(Guid teacherId, string? schoolYear = null);
    }
}