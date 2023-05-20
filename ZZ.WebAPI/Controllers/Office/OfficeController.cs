using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZZ.Commons;
using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.Office;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.Office
{
	[Route("[controller]/[action]")]
	[ApiController]
	public class OfficeController : ControllerBase
	{
		private readonly MyDbContext db;
		private readonly CommonRepository commonRepository;
		private readonly IdentityRepository identitRepository;

		public OfficeController(MyDbContext db, CommonRepository commonRepository, IdentityRepository identitRepository)
		{
			this.db = db;
			this.commonRepository = commonRepository;
			this.identitRepository = identitRepository;
		}

		/// <summary>
		/// 添加招聘需求申请
		/// </summary>
		/// <param name="hReq"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		[UnitOfWork(typeof(MyDbContext))]
		public async Task<IActionResult> AddHiringNeedApply(HiringNeedApplyRequest hReq)
		{
			// 获取申请人的 Guid
			string guid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			User user = await this.identitRepository.GetByUserAsync(guid);
			if (user == null)
			{
				return StatusCode(400, new { code = 400, msg = "发送此请求的用户不存在" });
			}

			Department d =  await this.commonRepository.FindByDepartmentIdAsync(hReq.DepartmentId);
			if (d == null)
			{
				return StatusCode(400,new { code = 400,msg = "部门不存在" });
			}

			Position p = await this.commonRepository.FindPositionAsync(hReq.PositionId);
			if (p == null)
			{
				return StatusCode(400, new { code = 400, msg = "职位不存在" });
			}

			if (hReq.NeedPersonCount < 0)
			{
				return StatusCode(400, new { code = 400, msg = "招聘人数应该大于零" });
			}

			// 获取int值对应的枚举类型
			ApplyType applyType = ForeachEnum.GetEnumFromInt<ApplyType>(hReq.ApplyType);
			if (applyType != ApplyType.HiringNeeds)
			{
				return StatusCode(400, new { code = 400, msg = "申请类型错误" });
			}

			HiringNeedApply h = new HiringNeedApply(user, applyType, hReq.Content, null, d, p, hReq.NeedPersonCount);
			this.db.HiringNeedApplys.Add(h);

			return StatusCode(400, new { code = 400, msg = "发布招聘需求成功" });
		}

		/// <summary>
		/// 获取所有招聘需求
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public dynamic GetHiringNeedApply()
		{
			List<HiringNeedApplyResponse> d =this.db.HiringNeedApplys.AsNoTracking().Select(e => new HiringNeedApplyResponse
			{
				Id = e.Id,
				applyUserName = e.User.UserName,
				MinDepartment = new MinDepartment() { Id = e.Id, DepartmentName =  e.Department.Name},
				MinPosition = new MinPosition() { Id = e.Position.Id,PositionName = e.Position.Name},
				Content = e.Content,
				NeedPersonCount = e.NeedPersonCount,
				HasPersonCount = e.HasPersonCount
			}).ToList();
			return d;
		}
	}
}
