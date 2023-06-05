using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZZ.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.JoinUs.Resume
{
	[Route("JoinUs/[controller]/[action]")]
	[ApiController]
	public class ResumeController : ControllerBase
	{
		private readonly MyDbContext db;
		private readonly IdentityRepository identityRepository;

		public ResumeController(MyDbContext db, IdentityRepository identityRepository)
		{
			this.db = db;
			this.identityRepository = identityRepository;
		}

		/// <summary>
		/// 获取应聘列表
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = "abc")]

		public IActionResult GetResumeList(ResumeListRequest req)
		{
			IQueryable<ZZ.Domain.Entities.JoinUs.Resume> sql = this.db.Resumes.AsNoTracking().Include(e => e.Department).Include(e => e.Position);

			// 是否搜索职位名称关键字
			if (req.NameKey != null && req.NameKey != "")
			{
				sql = sql.Where(e=> e.Name.Contains(req.NameKey));
			}

			
			JoinUsResult j = ForeachEnum.GetEnumFromInt<JoinUsResult>(req.JoinUsResult);

			// 关联查询当前处理人
			if (j == JoinUsResult.None)
			{
				sql = sql.Include(e => e.CheckUser);
			}

			// 根据应聘结果显示不同的简历
			sql = sql.Where(e => e.JoinUsResult == j);

			List<ZZ.Domain.Entities.JoinUs.Resume> data = sql.Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToList();

			int total = sql.Count();

			return StatusCode(200,new HttpResult(200,"获取成功",total,data));
			
		}

		/// <summary>
		/// 处理简历的环节
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost]
		[UnitOfWork(typeof(MyDbContext))]
		public async Task<IActionResult> ChangeJoinUsStep(ChangeJoinUsStepRequest req)
		{
			// 获取此次请求的用户Id
			Claim? claim = this.User.Claims.SingleOrDefault(e => e.Type == ClaimTypes.NameIdentifier);
			if (claim == null)
			{
				return StatusCode(400, new { code = 400, msg = "请携带正确的token" });
			}

			// 判断审核的用户是否存在
			User checkUser = await this.identityRepository.GetByUserIdAsync(claim.Value);
			if (checkUser == null)
			{
				return StatusCode(400, new { code = 400, msg = "发送此请求的用户不存在" });
			}

			// 判断此简历是否存在
			ZZ.Domain.Entities.JoinUs.Resume? r = await this.db.Resumes.FindAsync(req.ResumeId);
			if (r == null)
			{
				return StatusCode(400, new { code = 400, msg = "简历不存在" });
			}

			// 判断处理环节是否错误
			JoinUsStep s = ForeachEnum.GetEnumFromInt<JoinUsStep>(req.JoinUsStep);
			if (s == JoinUsStep.None)
			{
				return StatusCode(400, new { code = 400, msg = "此招聘环节不存在" });
			}

			// 改变实体状态
			r.JoinUsStep= s;
			r.CheckUser= checkUser;

			return StatusCode(200, new { code = 200, msg = "处理成功！" });
		}

		/// <summary>
		/// 处理简历的结果
		/// </summary>
		[HttpPost]
		[UnitOfWork(typeof(MyDbContext))]
		public async Task<IActionResult> ChangeJoinUsResult(ChangeJoinUsResultRequest req)
		{
			// 获取此次请求的用户Id
			Claim? claim = this.User.Claims.SingleOrDefault(e => e.Type == ClaimTypes.NameIdentifier);
			if (claim == null)
			{
				return StatusCode(400, new { code = 400, msg = "请携带正确的token" });
			}

			// 判断审核的用户是否存在
			User checkUser = await this.identityRepository.GetByUserIdAsync(claim.Value);
			if (checkUser == null)
			{
				return StatusCode(400, new { code = 400, msg = "发送此请求的用户不存在" });
			}

			// 判断此简历是否存在
			ZZ.Domain.Entities.JoinUs.Resume? r = await this.db.Resumes.Include(HiringNeedApply=>HiringNeedApply.HiringNeedApply).SingleOrDefaultAsync(Resume => Resume.Id == req.ResumeId);
			if (r == null)
			{
				return StatusCode(400, new { code = 400, msg = "简历不存在" });
			}

			// 判断是否决定了简历结果
			JoinUsResult j = ForeachEnum.GetEnumFromInt<JoinUsResult>(req.JoinUsResult);
			if (j == JoinUsResult.None)
			{
				return StatusCode(400, new { code = 400, msg = "处理错误" });
			}
			else if (j == JoinUsResult.入职)
			{
				// 如果此简历关联了招聘需求
				if (r.HiringNeedApply != null)
				{
					// 入职了才让招聘需求中的已招聘人数自增
					r.HiringNeedApply.HasPersonCount++;
				}
			}
			// 决定简历的结果。
			r.JoinUsResult = j;
			// 最后一次的处理人。
			r.CheckUser= checkUser;

			return StatusCode(200, new { code = 200, msg = "处理成功！" });
		}
	}
}
