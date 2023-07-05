using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.Office;

namespace ZZ.Infrastructure.Configs
{
	public class HiringNeedApplyConfig : IEntityTypeConfiguration<HiringNeedApply>
	{
		public void Configure(EntityTypeBuilder<HiringNeedApply> builder)
		{
			builder.ToTable("T_HiringNeedApplys");
			builder.HasOne<Department>(e => e.Department).WithMany().OnDelete(DeleteBehavior.Restrict);
			builder.HasOne<Position>(e => e.Position).WithMany().OnDelete(DeleteBehavior.Restrict);
		}
	}
}
