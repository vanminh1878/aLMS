using aLMS.Domain.StudentSubjectCommentEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Infrastructure.StudentSubjectCommentInfra
{
    public class StudentSubjectCommentConfiguration : IEntityTypeConfiguration<StudentSubjectComment>
    {
        public void Configure(EntityTypeBuilder<StudentSubjectComment> builder)
        {
            builder.ToTable("student_subject_comment");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.StudentEvaluationId).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.SubjectId).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.Comment).HasColumnType("text");

            builder.HasOne(x => x.StudentEvaluation).WithMany(e => e.SubjectComments).HasForeignKey(x => x.StudentEvaluationId);
            builder.HasOne(x => x.Subject).WithMany().HasForeignKey(x => x.SubjectId);
        }
    }
}
