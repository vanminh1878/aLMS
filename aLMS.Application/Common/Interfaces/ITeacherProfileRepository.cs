using aLMS.Domain.TeacherProfileEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface ITeacherProfileRepository
    {
        Task<IEnumerable<TeacherProfile>> GetAllTeacherProfilesAsync();
        Task<TeacherProfile> GetTeacherProfileByIdAsync(Guid userId);
        Task AddTeacherProfileAsync(TeacherProfile teacherProfile);
        Task UpdateTeacherProfileAsync(TeacherProfile teacherProfile);
        Task DeleteTeacherProfileAsync(Guid userId);
        Task<IEnumerable<TeacherProfile>> GetTeacherProfilesBySchoolIdAsync(Guid schoolId);
        Task<bool> TeacherProfileExistsAsync(Guid userId);
    }
}