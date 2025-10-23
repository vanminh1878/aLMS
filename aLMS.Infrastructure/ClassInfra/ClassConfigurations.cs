using aLMS.Domain.ClassEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
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

            builder.Property(x => x.GradeId)
                .HasColumnType("uuid");

            builder.Property(x => x.ClassName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.HasOne(x => x.Grade)
                .WithMany()
                .HasForeignKey(x => x.GradeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}