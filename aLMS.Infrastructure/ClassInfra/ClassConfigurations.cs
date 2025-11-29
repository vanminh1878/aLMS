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
            // Soft delete
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.DeletedAt);

            builder.HasMany(x => x.Subjects)
                .WithOne(s => s.Class)
                .HasForeignKey("ClassId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}