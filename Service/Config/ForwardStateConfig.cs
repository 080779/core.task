using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class ForwardStateConfig : IEntityTypeConfiguration<ForwardStateEntity>
    {
        public void Configure(EntityTypeBuilder<ForwardStateEntity> builder)
        {
            builder.ToTable("tb_forwardStates");
        }
    }
}
