using aLMS.Domain.BehaviourEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class BehaviourConfigurations : IEntityTypeConfiguration<Behaviour>
    {
        public void Configure(EntityTypeBuilder<Behaviour> builder)
        {
            builder.ToTable("behaviour");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Video)
                .HasMaxLength(500)
                .HasColumnType("varchar(500)");

            builder.Property(x => x.Result)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(x => x.Order)
                .HasColumnType("integer");

            builder.Property(x => x.StudentId)
                .HasColumnType("uuid");

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}