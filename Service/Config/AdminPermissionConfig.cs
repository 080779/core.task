using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class AdminPermissionConfig: IEntityTypeConfiguration<AdminPermissionEntity>
    {
        public void Configure(EntityTypeBuilder<AdminPermissionEntity> builder)
        {
            builder.ToTable("tb_adminPermissions");

            //builder.HasKey(a => new { a.AdminId, a.PermissionId });
            builder.HasOne(a => a.Admin).WithMany().IsRequired().HasForeignKey(a => a.AdminId);
            builder.HasOne(a => a.Permission).WithMany().IsRequired().HasForeignKey(a => a.PermissionId);
        }
    }
}
