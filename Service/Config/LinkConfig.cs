using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class LinkConfig : IEntityTypeConfiguration<LinkEntity>
    {
        public void Configure(EntityTypeBuilder<LinkEntity> builder)
        {
            builder.ToTable("tb_links");
        }
    }
}
