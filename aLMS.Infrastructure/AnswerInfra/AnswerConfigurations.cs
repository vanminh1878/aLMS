using aLMS.Domain.AnswerEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class AnswerConfigurations : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("answer");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.QuestionId)
                .HasColumnType("uuid");

            builder.Property(x => x.AnswerContent)
                .HasColumnType("text");

            builder.Property(x => x.IsCorrect)
                .HasColumnType("boolean")
                .HasDefaultValue(false);

            builder.Property(x => x.OrderNumber)
                .HasColumnType("integer");

            builder.HasOne(x => x.Question)
                .WithMany()
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}