using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.JoinUs;

namespace ZZ.Infrastructure.Configs
{
	public class RecordConfig : IEntityTypeConfiguration<Record>
	{
		public void Configure(EntityTypeBuilder<Record> builder)
		{
			builder.ToTable("T_Records");
			builder.HasOne<Position>(e => e.Position).WithMany().OnDelete(DeleteBehavior.Restrict);
		}
	}
}
