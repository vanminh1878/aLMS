using aLMS.Domain.StudentQualityEvaluationEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentQualityEvaluationInfra
{
    public class StudentQualityEvaluationConfiguration : IEntityTypeConfiguration<StudentQualityEvaluation>
    {
        public void Configure(EntityTypeBuilder<StudentQualityEvaluation> builder)
        {
            builder.ToTable("student_quality_evaluation");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.StudentEvaluationId).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.QualityId).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.QualityLevel).HasColumnType("varchar(20)");

            builder.HasOne(x => x.StudentEvaluation).WithMany(e => e.QualityEvaluations).HasForeignKey(x => x.StudentEvaluationId);
            builder.HasOne(x => x.Quality).WithMany().HasForeignKey(x => x.QualityId);
        }
    }
}
