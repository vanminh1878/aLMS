using aLMS.Domain.DepartmentEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("department");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.DepartmentName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(x => x.HeadId)
                .HasColumnType("uuid");

            builder.HasOne(x => x.Head)
                .WithMany()
                .HasForeignKey(x => x.HeadId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}