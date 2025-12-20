// aLMS.Infrastructure/ClassInfra/ClassConfigurations.cs
using aLMS.Domain.ClassEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.ClassInfra
{
    public class ClassConfigurations : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.ToTable("class");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.ClassName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Grade).IsRequired().HasMaxLength(20);
            builder.Property(x => x.SchoolYear).IsRequired().HasMaxLength(20);
            builder.Property(x => x.SchoolId)
                .IsRequired()
                .HasColumnType("uuid");
            builder.Property(x => x.HomeroomTeacherId)
                .HasColumnType("uuid")
                .IsRequired(false);
            // Soft delete
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.DeletedAt);

            builder.HasOne(x => x.HomeroomTeacher)
                .WithOne(t => t.HomeroomClass)
                .HasForeignKey<Class>(x => x.HomeroomTeacherId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Subjects)
                .WithOne(s => s.Class)
                .HasForeignKey("ClassId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.StudentEnrollments)
                .WithOne(e => e.Class)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}