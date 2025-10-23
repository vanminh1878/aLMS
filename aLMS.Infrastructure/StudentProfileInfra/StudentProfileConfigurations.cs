using aLMS.Domain.StudentProfileEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class StudentProfileConfigurations : IEntityTypeConfiguration<StudentProfile>
    {
        public void Configure(EntityTypeBuilder<StudentProfile> builder)
        {
            builder.ToTable("student_profile");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId)
                .HasColumnType("uuid");

            builder.Property(x => x.SchoolId)
                .HasColumnType("uuid");

            builder.Property(x => x.ClassId)
                .HasColumnType("uuid");

            builder.Property(x => x.EnrollDate)
                .HasColumnType("date");

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<StudentProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.School)
                .WithMany()
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Class)
                .WithMany()
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}