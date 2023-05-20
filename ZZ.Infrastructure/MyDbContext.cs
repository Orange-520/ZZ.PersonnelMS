using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.JoinUs;
using ZZ.Domain.Entities.Notice;
using ZZ.Domain.Entities.Office;

namespace ZZ.Infrastructure
{
	public class MyDbContext : IdentityDbContext<User,Role,Guid>
	{
		// JoinUs 模块
		public DbSet<Certificate> Certificates { get; set; }
		public DbSet<EducationHistory> EducationHistorys { get; set; }
		public DbSet<WorkHistory> WorkHistorys { get; set; }
		public DbSet<Record> Records { get; set; }
		public DbSet<Resume> Resumes { get; set; }

		// 公告模块
		public DbSet<Notice> Notices { get; set; }

		// 个人办公模块
		public DbSet<AskForLeaveApply> AskForLeaveApplys { get; set; }
		public DbSet<HiringNeedApply> HiringNeedApplys { get; set; }

		// 公共模块
		public DbSet<Department> Departments { get; set; }
		public DbSet<Position> Positions { get; set; }

		public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			// 使用了 Identity 框架，此处不能省略。
			base.OnModelCreating(builder);
			builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
			// 全局筛选器，过滤 IsDeleted = true 的数据。
			// ...
		}
	}
}
