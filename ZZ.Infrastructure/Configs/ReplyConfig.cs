using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.Office;

namespace ZZ.Infrastructure.Configs
{
	public class ReplyConfig : IEntityTypeConfiguration<Reply>
	{
		public void Configure(EntityTypeBuilder<Reply> builder)
		{
			builder.ToTable("T_Replys");
			builder.HasOne<User>(e=>e.User).WithMany().OnDelete(DeleteBehavior.Restrict);
			builder.HasOne<User>(e=>e.PublisherUser).WithMany().OnDelete(DeleteBehavior.Restrict);
		}
	}
}
