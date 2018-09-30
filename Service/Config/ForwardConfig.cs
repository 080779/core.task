using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class ForwardConfig : IEntityTypeConfiguration<ForwardEntity>
    {
        public void Configure(EntityTypeBuilder<ForwardEntity> builder)
        {
            builder.ToTable("tb_forwards");
        }
    }
}
