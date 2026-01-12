using aLMS.Domain.ClassSubjectEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.ClassSubjectTeacherInfra
{
    public class ClassSubjectTeacherConfiguration : IEntityTypeConfiguration<ClassSubjectTeacher>
    {
        public void Configure(EntityTypeBuilder<ClassSubjectTeacher> builder)
        {
            builder.ToTable("class_subject_teacher");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.ClassSubjectId)
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(x => x.TeacherId)
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(x => x.SchoolYear)
                .HasColumnType("varchar(20)")
                .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Relationships
            builder.HasOne(x => x.ClassSubject)
                .WithMany() // Không cần WithMany nếu không có collection trong ClassSubject
                .HasForeignKey(x => x.ClassSubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Teacher)
                .WithMany() // Nếu TeacherProfile có collection, dùng WithMany(t => t.ClassSubjectTeachers)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}