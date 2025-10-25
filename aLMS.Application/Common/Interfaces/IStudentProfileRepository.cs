using aLMS.Domain.StudentProfileEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStudentProfileRepository
    {
        Task<IEnumerable<StudentProfile>> GetAllStudentProfilesAsync();
        Task<StudentProfile> GetStudentProfileByIdAsync(Guid userId);
        Task AddStudentProfileAsync(StudentProfile studentProfile);
        Task UpdateStudentProfileAsync(StudentProfile studentProfile);
        Task DeleteStudentProfileAsync(Guid userId);
        Task<IEnumerable<StudentProfile>> GetStudentProfilesBySchoolIdAsync(Guid schoolId);
        Task<bool> StudentProfileExistsAsync(Guid userId);
    }
}