using aLMS.Application.Common.Dtos;
using aLMS.Domain.VirtualClassroomEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IVirtualClassroomRepository
    {
        Task<IEnumerable<VirtualClassroom>> GetAllAsync();
        Task AddAsync(VirtualClassroom entity);
        Task UpdateAsync(VirtualClassroom entity);
        Task DeleteAsync(Guid id);
        Task<VirtualClassroom?> GetByIdAsync(Guid id);
        Task<IEnumerable<VirtualClassroomDto>> GetByClassIdAsync(Guid classId, bool upcomingOnly);
        Task<IEnumerable<VirtualClassroom>> GetByStudentIdAsync(Guid studentId, bool upcomingOnly);
    }
}
