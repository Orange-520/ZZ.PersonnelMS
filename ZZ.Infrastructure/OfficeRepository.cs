using Microsoft.EntityFrameworkCore;
using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.JoinUs;
using ZZ.Domain.Entities.Office;
using ZZ.Domain.ValueObjects;

namespace ZZ.Infrastructure
{
	public class OfficeRepository
	{
		private readonly MyDbContext db;

		public OfficeRepository(MyDbContext db)
		{
			this.db = db;
		}

		/// <summary>
		/// 查询并返回一个申请
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="applyType"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<(bool,string, ApplyBase?)> FindApplyByIdAsync(ApplyType applyType,int id)
		{
			ApplyBase data = null;
			switch (applyType)
			{
				case ApplyType.None:
					break;
				case ApplyType.HiringNeeds:
					data = await this.db.HiringNeedApplys.Include(e=>e.User).Include(e=>e.Department).Include(e=>e.Position).FirstOrDefaultAsync(e=>e.Id == id);
					break;
				case ApplyType.GoOut:
					break;
				case ApplyType.AskForLeave:
					data = await this.db.AskForLeaveApplys.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id);
					break;
				case ApplyType.Dimission:
					break;
				case ApplyType.Reimbursement:
					break;
				default:
					break;
			}
			if (data == null)
			{
				return (false, "没有该类型的申请", null);
			}
			return (true,"查找成功",data);
		}

		/// <summary>
		/// 根据Id查找一个招聘需求
		/// </summary>
		/// <param name="hiringNeedApplyId"></param>
		/// <returns>存在招聘需求Id对应的实体则返回true</returns>
		public async Task<(bool,string,HiringNeedApply?)> FindHiringNeedApplyByIdAsync(int? hiringNeedApplyId)
		{
			if (hiringNeedApplyId == null)
			{
				return (false, "招聘需求Id不能为null", null);
			}
            HiringNeedApply h = await this.db.HiringNeedApplys.FindAsync(hiringNeedApplyId);
			if (h==null)
			{
				return (false, "没有此招聘需求", null);
			}
			return (true,"查找成功",h);
        }

		/// <summary>
		/// 批量修改招聘需求实体的需求部门属性为null
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public async Task UpdateHiringNeedApplyOfDepartmentSetNullAsync(Department d)
		{
			await this.db.HiringNeedApplys.Where(e => e.Department == d).ExecuteUpdateAsync(s=>s.SetProperty(b=>b.Department,b=>null));
		}

        /// <summary>
        /// 批量修改招聘需求实体的需求职位属性为null
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task UpdateHiringNeedApplyOfPositionSetNullAsync(Position p)
        {
            await this.db.HiringNeedApplys.Where(e => e.Position == p).ExecuteUpdateAsync(s => s.SetProperty(b => b.Position, b => null));
        }

        /// <summary>
        /// 批量修改招聘需求实体的需求部门属性为null，执行多次sql版本
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public void UpdateHiringNeedApplyOfDepartmentSetNull(Department d)
        {
            IQueryable<HiringNeedApply> hiringNeedApplys = this.db.HiringNeedApplys.Where(e => e.Department == d);
            foreach (var item in hiringNeedApplys)
            {
                item.Department = null;
            }
        }

        /// <summary>
        /// 批量修改招聘需求实体的需求职位属性为null，执行多次sql版本
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public void UpdateHiringNeedApplyOfPositionSetNull(Position p)
        {
            IQueryable<HiringNeedApply> hiringNeedApplys = this.db.HiringNeedApplys.Where(e => e.Position == p);
            foreach (var item in hiringNeedApplys)
            {
                item.Position = null;
            }
        }
    }
}
