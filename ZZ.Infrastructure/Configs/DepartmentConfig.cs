using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZZ.Domain.Entities.Commons;

namespace ZZ.Infrastructure.Configs
{
	public class DepartmentConfig : IEntityTypeConfiguration<Department>
	{
		public void Configure(EntityTypeBuilder<Department> builder)
		{
			builder.ToTable("T_Departments");
			builder.HasOne<Department>(e => e.ParentDepartmen).WithMany(e => e.ChildrenDepartment);
		}
	}
}
