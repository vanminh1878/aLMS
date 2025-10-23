using aLMS.Domain.SchoolEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class SchoolConfigurations : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.ToTable("school");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(x => x.Address)
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");

            builder.Property(x => x.Email)
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");

            builder.Property(x => x.Status)
                .HasColumnType("bit")
                .HasDefaultValue(true);
        }
    }
}