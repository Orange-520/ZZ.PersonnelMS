using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ZZ.Infrastructure
{
	public class MyDbContextDesignTimeFactory : IDesignTimeDbContextFactory<MyDbContext>
	{
		public MyDbContext CreateDbContext(string[] args)
		{
			DbContextOptionsBuilder<MyDbContext> builders = new DbContextOptionsBuilder<MyDbContext>();
			// 获取 Windows系统中环境变量的值(数据库连接字符串)
			string? conn = Environment.GetEnvironmentVariable("DefaultDB:ConnStr2");
			builders.UseSqlServer(conn);
			MyDbContext ctx = new MyDbContext(builders.Options);
			return ctx;
		}
	}
}
