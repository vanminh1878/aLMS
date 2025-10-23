using aLMS.Domain.SubjectEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class SubjectConfigurations : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("subject");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(x => x.Description)
                .HasColumnType("text");

            builder.Property(x => x.Category)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(x => x.ClassId)
                .HasColumnType("uuid");

            builder.HasOne(x => x.Class)
                .WithMany()
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}