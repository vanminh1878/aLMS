// src/Infrastructure/AccountInfra/AccountConfigurations.cs
using aLMS.Domain.AccountEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class AccountConfigurations : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("account"); 
            // === PRIMARY KEY ===
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()")
                .ValueGeneratedOnAdd();

            // === USERNAME (hoặc Email) ===
            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.HasIndex(x => x.Username)
                .IsUnique()
                .HasDatabaseName("IX_Accounts_Username");

            // === PASSWORD HASH ===
            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");

            // === STATUS (active/inactive) ===
            builder.Property(x => x.Status)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnType("boolean");

            // === REFRESH TOKEN ===
            builder.Property(x => x.RefreshToken)
                .IsRequired(false)
                .HasMaxLength(512)
                .HasColumnType("varchar(512)");

            builder.Property(x => x.RefreshTokenExpiry)
                .IsRequired(false)
                .HasColumnType("timestamp with time zone");

            // === SOFT DELETE (nếu dùng) ===
            // builder.Property(x => x.DeletedAt).HasColumnType("timestamp with time zone");
        }
    }
}