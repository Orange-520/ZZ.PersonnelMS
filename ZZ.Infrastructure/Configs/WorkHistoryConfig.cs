using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.JoinUs;

namespace ZZ.Infrastructure.Configs
{
	public class WorkHistoryConfig : IEntityTypeConfiguration<WorkHistory>
	{
		public void Configure(EntityTypeBuilder<WorkHistory> builder)
		{
			builder.ToTable("T_WorkHistorys");
		}
	}
}
