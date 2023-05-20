using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.Identity;

namespace ZZ.Infrastructure.Configs
{
	public class UserConfig : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("T_Users");
			// 取消主键默认为聚集索引的配置
			//builder.HasKey(e => e.Id).IsClustered(false) ;
		}
	}
}
