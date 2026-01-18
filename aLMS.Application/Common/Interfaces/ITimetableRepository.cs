using aLMS.Application.Common.Dtos;
using aLMS.Domain.TimetableEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface ITimetableRepository
    {
        Task AddAsync(Timetable timetable);
        Task UpdateAsync(Timetable timetable);
        Task DeleteAsync(Guid id);
        Task<Timetable?> GetByIdAsync(Guid id);
        Task<bool> HasConflictForClassAsync(Guid classId, short dayOfWeek, short periodNumber);
        Task<bool> HasConflictForTeacherAsync(Guid teacherId, short dayOfWeek, short periodNumber);
        Task<IEnumerable<TimetableDto>> GetByClassIdAsync(Guid classId, string? schoolYear);
        Task<IEnumerable<TimetableDto>> GetByTeacherIdAsync(Guid teacherId, string? schoolYear);
        Task<IEnumerable<Timetable>> GetByStudentIdAsync(Guid studentId, string? schoolYear); // Qua lớp của HS
    }
}