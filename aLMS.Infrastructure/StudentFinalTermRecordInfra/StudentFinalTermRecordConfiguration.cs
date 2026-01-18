using aLMS.Domain.StudentFinalTermRecordEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.StudentFinalTermRecordInfra
{
    public class StudentFinalTermRecordConfiguration : IEntityTypeConfiguration<StudentFinalTermRecord>
    {
        public void Configure(EntityTypeBuilder<StudentFinalTermRecord> builder)
        {
            builder.ToTable("student_final_term_record");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .ValueGeneratedNever();

            builder.Property(x => x.StudentProfileId)
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(x => x.ClassId)
                .HasColumnType("uuid")
                .IsRequired(false);

            builder.Property(x => x.FinalScore)
                .HasColumnType("numeric(5,2)")
                .IsRequired(false);

            builder.Property(x => x.FinalEvaluation)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.Comment)
                .HasColumnType("text")
                .IsRequired(false);
            builder.Property(x => x.SubjectId)        
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("timestamptz");

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("timestamptz")
                .IsRequired(false);

            // Quan hệ
            builder.HasOne(x => x.StudentProfile)
                .WithMany()
                .HasForeignKey(x => x.StudentProfileId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}