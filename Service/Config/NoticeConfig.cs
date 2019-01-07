using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class NoticeConfig : IEntityTypeConfiguration<NoticeEntity>
    {
        public void Configure(EntityTypeBuilder<NoticeEntity> builder)
        {
            builder.ToTable("tb_notices");
        }
    }
}
