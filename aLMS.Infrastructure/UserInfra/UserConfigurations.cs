using aLMS.Domain.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(x => x.DateOfBirth)
                .HasColumnType("date");

            builder.Property(x => x.Gender)
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");

            builder.Property(x => x.Email)
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Address)
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");

            builder.Property(x => x.SchoolId)
                .HasColumnType("uuid");

            builder.Property(x => x.AccountId)
                .HasColumnType("uuid");

            builder.Property(x => x.RoleId)
                .HasColumnType("uuid");

            builder.HasOne(x => x.School)
                .WithMany()
                .HasForeignKey(x => x.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Account)
                .WithOne()
                .HasForeignKey<User>(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}