using aLMS.Domain.NotificationEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.NotificationInfra
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("notification");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Title)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(x => x.Content)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(x => x.TargetType)
                .HasColumnType("varchar(30)")
                .IsRequired();

            builder.Property(x => x.TargetId)
                .HasColumnType("uuid")
                .IsRequired(false);

            builder.Property(x => x.CreatedBy)
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(x => x.SchoolId)
                .HasColumnType("uuid")
                .IsRequired(false);

            builder.Property(x => x.IsRead)
                .HasColumnType("boolean")
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}