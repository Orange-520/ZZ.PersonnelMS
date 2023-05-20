namespace ZZ.Infrastructure
{
	public class JoinUsRepository
	{
		private readonly MyDbContext db;

		public JoinUsRepository(MyDbContext db)
		{
			this.db = db;
		}
	}
}
