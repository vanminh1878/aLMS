using aLMS.Domain.VirtualClassroomEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.VirtualClassroomInfra
{
    public class VirtualClassroomConfiguration : IEntityTypeConfiguration<VirtualClassroom>
    {
        public void Configure(EntityTypeBuilder<VirtualClassroom> builder)
        {
            builder.ToTable("virtual_classroom");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.ClassId).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.SubjectId).HasColumnType("uuid").IsRequired(false);
            builder.Property(x => x.TimetableId).HasColumnType("uuid").IsRequired(false);

            builder.Property(x => x.DayOfWeek).HasColumnType("smallint").IsRequired(false);
            builder.Property(x => x.PeriodNumber).HasColumnType("smallint").IsRequired(false);

            builder.Property(x => x.Title).HasColumnType("varchar(150)").IsRequired();
            builder.Property(x => x.MeetingUrl).HasColumnType("varchar(500)").IsRequired();
            builder.Property(x => x.MeetingId).HasColumnType("varchar(100)");
            builder.Property(x => x.Password).HasColumnType("varchar(50)");

            builder.Property(x => x.StartTime).HasColumnType("timestamptz").IsRequired();
            builder.Property(x => x.EndTime).HasColumnType("timestamptz").IsRequired();

            builder.Property(x => x.CreatedBy).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.IsRecurring).HasColumnType("boolean").HasDefaultValue(false);

            builder.Property(x => x.CreatedAt).HasColumnType("timestamptz").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.UpdatedAt).HasColumnType("timestamptz").HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(x => x.Class)
                .WithMany()
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Subject)
                .WithMany()
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Timetable)
                .WithMany()
                .HasForeignKey(x => x.TimetableId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.CreatedByTeacher)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}