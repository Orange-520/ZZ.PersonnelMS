using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;

namespace ZZ.WebAPI.Controllers.JoinUs.HiringNeedApply
{
	[Route("JoinUs/[controller]/[action]")]
	[ApiController]
	public class HiringNeedApplyController : ControllerBase
	{
		private readonly MyDbContext db;

		public HiringNeedApplyController(MyDbContext db)
		{
			this.db = db;
		}

		/// <summary>
		/// 获取所有招聘需求
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public IActionResult GetHiringNeedApply(HiringNeedsApplyRequest req)
		{
			IQueryable<ZZ.Domain.Entities.Office.HiringNeedApply> sql = this.db.HiringNeedApplys.AsNoTracking().Where(e => e.CheckState == CheckState.Yes);

			// 是否搜索职位关键字
			if (req.PositionWordKey != null && req.PositionWordKey != "")
			{
				sql = sql.Where(e => e.Position.Name.Contains(req.PositionWordKey));
			}

			IQueryable<HiringNeedApplyResponse> list = sql.Select(e => new HiringNeedApplyResponse
			{
				Id = e.Id,
				applyUserName = e.User.UserName,
				MinDepartment = new MinDepartment() { Id = e.Id, DepartmentName = e.Department.Name },
				MinPosition = new MinPosition() { Id = e.Position.Id, PositionName = e.Position.Name },
				Content = e.Content,
				NeedPersonCount = e.NeedPersonCount,
				HasPersonCount = e.HasPersonCount
			});

			int total = list.Count();

			List<HiringNeedApplyResponse> data = list.Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToList();

			return StatusCode(200, new HttpResult(200,"请求成功",total,data));
		}
	}
}
