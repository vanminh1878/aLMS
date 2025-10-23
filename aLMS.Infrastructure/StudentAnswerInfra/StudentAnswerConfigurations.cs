using aLMS.Domain.StudentAnswerEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class StudentAnswerConfigurations : IEntityTypeConfiguration<StudentAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentAnswer> builder)
        {
            builder.ToTable("student_answer");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.StudentExerciseId)
                .HasColumnType("uuid");

            builder.Property(x => x.QuestionId)
                .HasColumnType("uuid");

            builder.Property(x => x.AnswerId)
                .HasColumnType("uuid");

            builder.Property(x => x.AnswerText)
                .HasColumnType("text");

            builder.Property(x => x.IsCorrect)
                .HasColumnType("bit")
                .HasDefaultValue(false);

            builder.Property(x => x.SubmittedAt)
                .HasColumnType("timestamp");

            builder.HasOne(x => x.StudentExercise)
                .WithMany()
                .HasForeignKey(x => x.StudentExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Question)
                .WithMany()
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Answer)
                .WithMany()
                .HasForeignKey(x => x.AnswerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}