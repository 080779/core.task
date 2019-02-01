using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class ShopCartConfig : IEntityTypeConfiguration<ShopCartEntity>
    {
        public void Configure(EntityTypeBuilder<ShopCartEntity> builder)
        {
            builder.ToTable("tb_shopCarts");
        }
    }
}
