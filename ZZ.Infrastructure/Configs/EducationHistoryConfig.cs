using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.JoinUs;

namespace ZZ.Infrastructure.Configs
{
	public class EducationHistoryConfig : IEntityTypeConfiguration<EducationHistory>
	{
		public void Configure(EntityTypeBuilder<EducationHistory> builder)
		{
			builder.ToTable("T_EducationHistorys");
		}
	}
}
