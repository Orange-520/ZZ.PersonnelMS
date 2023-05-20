using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.Office;

namespace ZZ.Infrastructure.Configs
{
	public class AskForLeaveApplyConfig : IEntityTypeConfiguration<AskForLeaveApply>
	{
		public void Configure(EntityTypeBuilder<AskForLeaveApply> builder)
		{
			builder.ToTable("T_AskForLeaveApplys");
			builder.HasOne<User>(e=>e.CheckUser).WithMany().OnDelete(DeleteBehavior.Restrict);
		}
	}
}
