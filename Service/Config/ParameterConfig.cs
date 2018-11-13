using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class ParameterConfig : IEntityTypeConfiguration<ParameterEntity>
    {
        public void Configure(EntityTypeBuilder<ParameterEntity> builder)
        {
            builder.ToTable("tb_parameters");
        }
    }
}
