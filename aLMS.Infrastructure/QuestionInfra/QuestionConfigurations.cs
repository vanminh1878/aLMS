using aLMS.Domain.QuestionEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class QuestionConfigurations : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("question");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.ExerciseId)
                .HasColumnType("uuid");

            builder.Property(x => x.QuestionContent)
                .HasColumnType("text");

            builder.Property(x => x.QuestionImage)
                .HasMaxLength(500)
                .HasColumnType("varchar(500)");

            builder.Property(x => x.QuestionType)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(x => x.OrderNumber)
                .HasColumnType("integer");

            builder.Property(x => x.Score)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.Explanation)
                .HasColumnType("text");

            builder.HasOne(x => x.Exercise)
                .WithMany()
                .HasForeignKey(x => x.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}