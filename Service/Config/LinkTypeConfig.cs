using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class LinkTypeConfig : IEntityTypeConfiguration<LinkTypeEntity>
    {
        public void Configure(EntityTypeBuilder<LinkTypeEntity> builder)
        {
            builder.ToTable("tb_linkTypes");
        }
    }
}
