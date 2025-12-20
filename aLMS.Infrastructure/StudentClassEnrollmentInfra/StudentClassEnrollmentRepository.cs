using aLMS.Application.Common.Interfaces;
using aLMS.Domain.StudentClassEnrollmentEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentClassEnrollmentInfra
{
    public class StudentClassEnrollmentRepository : IStudentClassEnrollmentRepository
    {
        private readonly DbContext _context;
        private readonly string _connectionString;
        public StudentClassEnrollmentRepository(DbContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task AddEnrollmentAsync(Guid studentProfileId, Guid classId)
        {
            var enrollment = new StudentClassEnrollment
            {
                StudentProfileId = studentProfileId,
                ClassId = classId
            };
            _context.Set<StudentClassEnrollment>().Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task AddEnrollmentsAsync(IEnumerable<(Guid studentProfileId, Guid classId)> enrollments)
        {
            var entities = enrollments.Select(e => new StudentClassEnrollment
            {
                StudentProfileId = e.studentProfileId,
                ClassId = e.classId
            });
            _context.Set<StudentClassEnrollment>().AddRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
