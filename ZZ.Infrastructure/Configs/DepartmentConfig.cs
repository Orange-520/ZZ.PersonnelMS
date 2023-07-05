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
            // DeleteBehavior.Restrict：当删除被关联的实体时，将抛出异常。
			// 设置一对多的关系，并外键属性可以为null
            builder.HasOne<Department>(e => e.ParentDepartmen).WithMany(e=>e.ChildrenDepartment).HasForeignKey("ParentDepartmentId").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        }
	}
}
