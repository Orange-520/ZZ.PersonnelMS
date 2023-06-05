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
			// 获取通过审核申请的招聘需求
			IQueryable<ZZ.Domain.Entities.Office.HiringNeedApply> sql = this.db.HiringNeedApplys.AsNoTracking().Where(e => e.CheckState == CheckState.Yes);

			// 是否搜索职位关键字
			if (req.PositionWordKey != null && req.PositionWordKey != "")
			{
				sql = sql.Where(e => e.Position.Name.Contains(req.PositionWordKey));
			}

			// 深度查询，查询关联招聘需求的简历，主要获取简历实体中的应聘部门、应聘职位以及简历处理人
			// 官方：这并不意味着会获得冗余联接查询，在大多数情况下，EF Core 会在生成 SQL 时合并相应的联接查询。
			sql = sql.Include(HiringNeedApply => HiringNeedApply.CurrentResumes).ThenInclude(CurrentResumes => CurrentResumes.Department);

			sql = sql.Include(HiringNeedApply => HiringNeedApply.CurrentResumes).ThenInclude(CurrentResumes => CurrentResumes.Position);

			sql = sql.Include(HiringNeedApply => HiringNeedApply.CurrentResumes).ThenInclude(CurrentResumes => CurrentResumes.CheckUser);

			IQueryable<HiringNeedApplyResponse> list = sql.Select(e => new HiringNeedApplyResponse
			{
				Id = e.Id,
				applyUserName = e.User.UserName,
				MinDepartment = new MinDepartment() { Id = e.Department.Id, DepartmentName = e.Department.Name },
				MinPosition = new MinPosition() { Id = e.Position.Id, PositionName = e.Position.Name },
				Content = e.Content,
				NeedPersonCount = e.NeedPersonCount,
				HasPersonCount = e.HasPersonCount,
				// 过滤掉应聘列表简历中为入职、人才库、资料库、不合格的。
				ResumeList = (List<Domain.Entities.JoinUs.Resume>)e.CurrentResumes.Where(b => b.JoinUsResult == JoinUsResult.None)
			});

			int total = list.Count();

			List<HiringNeedApplyResponse> data = list.Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToList();

			return StatusCode(200, new HttpResult(200,"请求成功",total,data));
		}
	}
}
