using aLMS.Application.Common.Interfaces;
using aLMS.Domain.SchoolEntity;
using aLMS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.SchoolInfra
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly aLMSDbContext _context;

        public SchoolRepository(aLMSDbContext context)
        {
            _context = context;
        }

        public async Task<List<School>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Schools
                .Include(s => s.Grades)
                .Include(s => s.Users)
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<School?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Schools
                .Include(s => s.Grades)
                .Include(s => s.Users)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task AddAsync(School school, CancellationToken cancellationToken = default)
        {
            await _context.Schools.AddAsync(school, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(School school, CancellationToken cancellationToken = default)
        {
            _context.Schools.Update(school);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var school = await _context.Schools.FindAsync(new object[] { id }, cancellationToken);
            if (school != null)
            {
                _context.Schools.Remove(school);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Schools
                .AnyAsync(s => s.Name.ToLower() == name.ToLower(), cancellationToken);
        }
    }
}
