using Microsoft.EntityFrameworkCore;
using ZZ.Domain.Entities.Office;
using ZZ.Domain.ValueObjects;

namespace ZZ.Infrastructure
{
	public class OfficeRepository
	{
		private readonly MyDbContext dbContext;

		public OfficeRepository(MyDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		/// <summary>
		/// 查询并返回一个申请
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="applyType"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<ApplyBase> FindApplyAsync(ApplyType applyType,int id)
		{
			ApplyBase data = null;
			switch (applyType)
			{
				case ApplyType.None:
					break;
				case ApplyType.HiringNeeds:
					data = await this.dbContext.HiringNeedApplys.Include(e=>e.User).SingleOrDefaultAsync(e=>e.Id == id);
					break;
				case ApplyType.GoOut:
					break;
				case ApplyType.AskForLeave:
					data = await this.dbContext.AskForLeaveApplys.Include(e => e.User).SingleOrDefaultAsync(e => e.Id == id);
					break;
				case ApplyType.Dimission:
					break;
				case ApplyType.Reimbursement:
					break;
				default:
					break;
			}
			return data;
		}
	
		
	}
}
