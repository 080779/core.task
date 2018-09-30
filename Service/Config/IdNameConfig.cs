using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class IdNameConfig : IEntityTypeConfiguration<IdNameEntity>
    {
        public void Configure(EntityTypeBuilder<IdNameEntity> builder)
        {
            builder.ToTable("tb_idNames");
        }
    }
}
