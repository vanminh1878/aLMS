using aLMS.Domain.StudentEvaluationEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.StudentEvaluationInfra
{
    public class StudentEvaluationConfiguration : IEntityTypeConfiguration<StudentEvaluation>
    {
        public void Configure(EntityTypeBuilder<StudentEvaluation> builder)
        {
            builder.ToTable("student_evaluation");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.StudentId).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.ClassId).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.Semester).HasColumnType("varchar(10)");
            builder.Property(x => x.SchoolYear).HasColumnType("varchar(20)");
            builder.Property(x => x.FinalScore).HasColumnType("numeric(5,2)");
            builder.Property(x => x.Level).HasColumnType("varchar(20)");
            builder.Property(x => x.GeneralComment).HasColumnType("text");
            builder.Property(x => x.FinalEvaluation).HasColumnType("varchar(20)");
            builder.Property(x => x.CreatedBy).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnType("timestamptz").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.UpdatedAt).HasColumnType("timestamptz").HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(x => x.Student).WithMany().HasForeignKey(x => x.StudentId);
            builder.HasOne(x => x.Class).WithMany().HasForeignKey(x => x.ClassId);
            builder.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
        }
    }
}