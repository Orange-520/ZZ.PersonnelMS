using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZZ.Domain.Entities.Commons;
using ZZ.Infrastructure;

namespace ZZ.WebAPI.Controllers.JoinUs.Record
{
	[Route("JoinUs/[controller]/[action]")]
	[ApiController]
	public class RecordController : ControllerBase
	{
		private readonly MyDbContext db;

		public RecordController(MyDbContext db)
		{
			this.db = db;
		}

		[HttpPost]
		public IActionResult GetRecordList(RecordListRequest req)
		{
			IQueryable<ZZ.Domain.Entities.JoinUs.Record> sql = this.db.Records.AsNoTracking().Include(e => e.User).Include(e=>e.Department).Include(e=>e.Position);

			// 判断是否搜索登录名或档案人名
			if (req.NameKey != null && req.NameKey != "")
			{
				// 登录名中存在 或者 姓名中存在
				sql = sql.Where(e=>e.User.UserName.Contains(req.NameKey) || e.Name.Contains(req.NameKey));
			}

			// 判断是否搜索部门
			if (req.DepartmentId != null)
			{
				sql = sql.Where(e => e.Department.Id == req.DepartmentId);
			}

			// 是否搜索职位
			if (req.PositionId != null)
			{
				sql = sql.Where(e => e.Position.Id == req.PositionId);
			}

			int total = sql.Count();
			List<ZZ.Domain.Entities.JoinUs.Record> data = sql.Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToList();

			return Ok(new HttpResult(200,"获取成功",total, data));
		}
	}
}
