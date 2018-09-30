using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class CollectConfig : IEntityTypeConfiguration<CollectEntity>
    {
        public void Configure(EntityTypeBuilder<CollectEntity> builder)
        {
            builder.ToTable("tb_collects");
        }
    }
}
