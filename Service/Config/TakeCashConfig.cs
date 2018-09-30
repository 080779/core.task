using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class TakeCashConfig : IEntityTypeConfiguration<TakeCashEntity>
    {
        public void Configure(EntityTypeBuilder<TakeCashEntity> builder)
        {
            builder.ToTable("tb_takeCashs");
        }
    }
}
