using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Entity;

namespace Service.Config
{
    class JournalConfig : IEntityTypeConfiguration<JournalEntity>
    {
        public void Configure(EntityTypeBuilder<JournalEntity> builder)
        {
            builder.ToTable("tb_journals");
        }
    }
}
