using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class AdminLogConfig : IEntityTypeConfiguration<AdminLogEntity>
    {
        public void Configure(EntityTypeBuilder<AdminLogEntity> builder)
        {
            builder.ToTable("tb_adminLogs");
        }
    }
}
