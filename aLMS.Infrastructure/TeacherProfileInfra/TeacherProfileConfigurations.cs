using aLMS.Domain.TeacherProfileEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class TeacherProfileConfigurations : IEntityTypeConfiguration<TeacherProfile>
    {
        public void Configure(EntityTypeBuilder<TeacherProfile> builder)
        {
            builder.ToTable("teacher_profile");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId)
                .HasColumnType("uuid");

            builder.Property(x => x.SchoolId)
                .HasColumnType("uuid");

            builder.Property(x => x.DepartmentId)
                .HasColumnType("uuid");

            builder.Property(x => x.HireDate)
                .HasColumnType("date");

            builder.Property(x => x.Specialization)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<TeacherProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.School)
                .WithMany()
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}