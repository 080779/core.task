using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class ParameterTypeConfig : IEntityTypeConfiguration<ParameterTypeEntity>
    {
        public void Configure(EntityTypeBuilder<ParameterTypeEntity> builder)
        {
            builder.ToTable("tb_parameterTypes");
        }
    }
}
