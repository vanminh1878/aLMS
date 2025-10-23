using aLMS.Domain.LessonEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class LessonConfigurations : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("lesson");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(x => x.Description)
                .HasColumnType("text");

            builder.Property(x => x.ResourceType)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(x => x.Content)
                .HasColumnType("text");

            builder.Property(x => x.IsRequired)
                .HasColumnType("boolean")
                .HasDefaultValue(false);

            builder.Property(x => x.TopicId)
                .HasColumnType("uuid");

            builder.HasOne(x => x.Topic)
                .WithMany()
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}