using aLMS.Domain.RolePermissionEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aLMS.Infrastructure.AccountInfra
{
    public class RolePermissionConfigurations : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("role_permission");

            builder.HasKey(x => new { x.RoleId, x.PermissionId });

            builder.Property(x => x.RoleId)
                .HasColumnType("uuid");

            builder.Property(x => x.PermissionId)
                .HasColumnType("uuid");

            builder.HasOne(x => x.Role)
                .WithMany(x=> x.RolePermissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Permission)
                .WithMany(x=> x.RolePermissions)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}