using aLMS.Domain.ParentProfileEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class ParentProfileConfigurations : IEntityTypeConfiguration<ParentProfile>
    {
        public void Configure(EntityTypeBuilder<ParentProfile> builder)
        {
            builder.ToTable("parent_profile");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId)
                .HasColumnType("uuid");

            builder.Property(x => x.StudentId)
                .HasColumnType("uuid");

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<ParentProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}