using ZZ.Domain.Entities.Office;

namespace ZZ.Infrastructure
{
	public class OfficeRepository
	{
		private readonly MyDbContext dbContext;

		public OfficeRepository(MyDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

	}
}
