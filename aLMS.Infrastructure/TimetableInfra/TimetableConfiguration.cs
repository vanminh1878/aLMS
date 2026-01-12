using aLMS.Domain.TimetableEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.TimetableInfra
{
    public class TimetableConfiguration : IEntityTypeConfiguration<Timetable>
    {
        public void Configure(EntityTypeBuilder<Timetable> builder)
        {
            builder.ToTable("timetable");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.ClassId).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.SubjectId).HasColumnType("uuid").IsRequired();
            builder.Property(x => x.TeacherId).HasColumnType("uuid").IsRequired();

            builder.Property(x => x.DayOfWeek).HasColumnType("smallint").IsRequired();
            builder.Property(x => x.PeriodNumber).HasColumnType("smallint").IsRequired();

            builder.Property(x => x.StartTime).HasColumnType("time");
            builder.Property(x => x.EndTime).HasColumnType("time");
            builder.Property(x => x.Room).HasColumnType("varchar(50)");
            builder.Property(x => x.SchoolYear).HasColumnType("varchar(20)");

            builder.Property(x => x.CreatedAt).HasColumnType("timestamptz").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.UpdatedAt).HasColumnType("timestamptz").HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(x => x.Class)
                .WithMany()
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Subject)
                .WithMany()
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}