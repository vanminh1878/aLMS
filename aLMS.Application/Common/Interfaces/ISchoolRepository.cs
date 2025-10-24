using aLMS.Domain.SchoolEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Application.Common.Interfaces
{
    public interface ISchoolRepository
    {
        Task<List<School>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<School?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(School school, CancellationToken cancellationToken = default);
        Task UpdateAsync(School school, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
