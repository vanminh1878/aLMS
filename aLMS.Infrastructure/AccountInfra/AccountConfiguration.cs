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

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.HasIndex(x => x.Username)
                .IsUnique()
                .HasDatabaseName("IX_Accounts_Username");

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnType("boolean");

            builder.Property(x => x.RefreshToken)
                .IsRequired(false)
                .HasMaxLength(512)
                .HasColumnType("varchar(512)");

            builder.Property(x => x.RefreshTokenExpiry)
                .IsRequired(false)
                .HasColumnType("timestamp with time zone");
        }
    }
}
