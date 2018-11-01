using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class IdNameTypeConfig : IEntityTypeConfiguration<IdNameTypeEntity>
    {
        public void Configure(EntityTypeBuilder<IdNameTypeEntity> builder)
        {
            builder.ToTable("tb_idNameTypes");
        }
    }
}
