using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.JoinUs;

namespace ZZ.Infrastructure.Configs
{
	public class ResumeConfig : IEntityTypeConfiguration<Resume>
	{
		public void Configure(EntityTypeBuilder<Resume> builder)
		{
			builder.ToTable("T_Resumes");

			builder.Property(e => e.SalaryExpectation).HasColumnType("money");
			builder.HasOne<Position>(e => e.Position).WithMany().OnDelete(DeleteBehavior.Restrict);
		}
	}
}
