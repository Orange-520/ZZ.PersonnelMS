using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.Notice;

namespace ZZ.Infrastructure.Configs
{
	public class NoticeConfig : IEntityTypeConfiguration<Notice>
	{
		public void Configure(EntityTypeBuilder<Notice> builder)
		{
			builder.ToTable("T_Notices");
		}
	}
}
