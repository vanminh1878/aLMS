using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface IStudentClassEnrollmentRepository
    {
        Task AddEnrollmentAsync(Guid studentProfileId, Guid classId);
        Task AddEnrollmentsAsync(IEnumerable<(Guid studentProfileId, Guid classId)> enrollments);
        Task<bool> IsStudentInClassAsync(Guid studentUserId, Guid classId);
    }
}
