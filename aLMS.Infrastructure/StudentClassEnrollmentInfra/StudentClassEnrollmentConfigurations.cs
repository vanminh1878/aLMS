using aLMS.Domain.StudentClassEnrollmentEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentClassEnrollmentInfra
{
    public class StudentClassEnrollmentConfigurations : IEntityTypeConfiguration<StudentClassEnrollment>
    {
        public void Configure(EntityTypeBuilder<StudentClassEnrollment> builder)
        {
            builder.ToTable("student_class_enrollment");

            builder.HasKey(e => new { e.StudentProfileId, e.ClassId });

            builder.HasOne(e => e.StudentProfile)
                .WithMany(s => s.ClassEnrollments)
                .HasForeignKey(e => e.StudentProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Class)
                .WithMany(c => c.StudentEnrollments)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
