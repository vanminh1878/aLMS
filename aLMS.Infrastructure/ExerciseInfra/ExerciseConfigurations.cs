using aLMS.Domain.ExerciseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class ExerciseConfigurations : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.ToTable("exercise");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(x => x.ExerciseFile)
                .HasMaxLength(500)
                .HasColumnType("varchar(500)");

            builder.Property(x => x.HasTimeLimit)
                .HasColumnType("bit")
                .HasDefaultValue(false);

            builder.Property(x => x.TimeLimit)
                .HasColumnType("integer");

            builder.Property(x => x.QuestionLayout)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(x => x.OrderNumber)
                .HasColumnType("integer");

            builder.Property(x => x.TotalScore)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.LessonId)
                .HasColumnType("uuid");

            builder.HasOne(x => x.Lesson)
                .WithMany()
                .HasForeignKey(x => x.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}