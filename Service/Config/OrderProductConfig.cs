using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class OrderProductConfig : IEntityTypeConfiguration<OrderProductEntity>
    {
        public void Configure(EntityTypeBuilder<OrderProductEntity> builder)
        {
            builder.ToTable("tb_orderProducts");
        }
    }
}
