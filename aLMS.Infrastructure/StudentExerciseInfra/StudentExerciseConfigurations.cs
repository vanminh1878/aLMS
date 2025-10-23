using aLMS.Domain.StudentExerciseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class StudentExerciseConfigurations : IEntityTypeConfiguration<StudentExercise>
    {
        public void Configure(EntityTypeBuilder<StudentExercise> builder)
        {
            builder.ToTable("student_exercise");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.StudentId)
                .HasColumnType("uuid");

            builder.Property(x => x.ExerciseId)
                .HasColumnType("uuid");

            builder.Property(x => x.StartTime)
                .HasColumnType("timestamp");

            builder.Property(x => x.EndTime)
                .HasColumnType("timestamp");

            builder.Property(x => x.Score)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.IsCompleted)
                .HasColumnType("bit")
                .HasDefaultValue(false);

            builder.Property(x => x.AttemptNumber)
                .HasColumnType("integer");

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Exercise)
                .WithMany()
                .HasForeignKey(x => x.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}