using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.Commons;

namespace ZZ.Infrastructure.Configs
{
    public class PositionConfig : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("T_Positions");
            // 级联删除，当删除主实体时，自动删除所有相关联的实体。
            builder.HasOne<Department>(e=>e.Department).WithMany().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
