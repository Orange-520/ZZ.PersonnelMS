using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZZ.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.Office;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.Office.Approve
{
	[Route("Office/[controller]/[action]")]
	[ApiController]
	public class ApproveController : ControllerBase
	{
		private readonly MyDbContext db;
		private readonly IdentityRepository identitRepository;
		private readonly OfficeRepository officeRepository;

		public ApproveController(MyDbContext db, IdentityRepository identitRepository, OfficeRepository officeRepository)
		{
			this.db = db;
			this.identitRepository = identitRepository;
			this.officeRepository = officeRepository;
		}

		/// <summary>
		/// 获取待审核的申请
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult GetAllCheckApply()
		{
			List<ApplyResponse> data = new List<ApplyResponse>();

			// 内存中加载
			IEnumerable<ApplyResponse> list1 = this.db.AskForLeaveApplys.Where(e => e.CheckState == CheckState.Wait).Select(e => new ApplyResponse
			{
				Id = e.Id,
				User = e.User,
				CreateTime = e.CreationTime,
				ApplyType = e.ApplyType,
				Content = e.Content
			});
			foreach (ApplyResponse item in list1)
			{
				data.Add(item);
			}

			IEnumerable<ApplyResponse> list2 = this.db.HiringNeedApplys.Where(e => e.CheckState == CheckState.Wait).Select(e => new ApplyResponse
			{
				Id = e.Id,
				User = e.User,
				CreateTime = e.CreationTime,
				ApplyType = e.ApplyType,
				Content = e.Content
			});
			foreach (ApplyResponse item in list2)
			{
				data.Add(item);
			}

			//data.Skip((page.PageIndex - 1) * page.PageSize).Take(page.PageSize);

			return StatusCode(200, new { code = 200, msg = "请求成功", data });
		}
		
		/// <summary>
		/// 审核申请
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[UnitOfWork(typeof(MyDbContext))]
		public async Task<IActionResult> CheckApply(CheckApplyRequest req)
		{
			// 获取申请人的 Guid
			string checkUserGuid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			User checkUser = await this.identitRepository.GetByUserIdAsync(checkUserGuid);
			if (checkUser == null)
			{
				return StatusCode(400, new { code = 400, msg = "处理此审核的用户不存在" });
			}

			// 检查申请类型是否正确
			ApplyType applyType = ForeachEnum.GetEnumFromInt<ApplyType>(req.ApplyType);
			if (applyType == ApplyType.None)
			{
				return StatusCode(400, new { code = 400, msg = "申请类型错误" });
			}

			// 检查审核类型是否正确
			CheckState checkState = ForeachEnum.GetEnumFromInt<CheckState>(req.CheckState);
			if (checkState == CheckState.None)
			{
				return StatusCode(400, new { code = 400, msg = "审核类型错误" });
			}

			// 获取申请实体
			ApplyBase a = await this.officeRepository.FindApplyAsync(applyType, req.Id);
			if (a == null)
			{
				return StatusCode(400, new { code = 400, msg = "未找到该申请" });
			}
			// 修改实体状态
			a.AlterCheckState(checkState);
			a.CheckUser = checkUser;
			a.ListModificationTime = DateTime.Now;

			// 回复表
			Reply r = new Reply(a.User, checkUser, ReplyType.个人申请, req.ReplyTitle, req.ReplyContent);
			this.db.Replys.Add(r);

			return StatusCode(200, new { code = 200, msg = "审核成功" });

		}

		/// <summary>
		/// 获取申请详情
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> GetApplyDetail(ApplyDetailRequest req)
		{
			// 获取int值对应的枚举类型
			ApplyType applyType = ForeachEnum.GetEnumFromInt<ApplyType>(req.ApplyType);
			if (applyType == ApplyType.None)
			{
				return StatusCode(400, new { code = 400, msg = "申请类型错误" });
			}

			// 申请详情
			ApplyBase data = await this.officeRepository.FindApplyAsync(applyType, req.Id);
			if (data == null)
			{
				return StatusCode(400, new { code = 400, msg = "找不到id对应的申请详情", data });
			}

			return StatusCode(200, new { code = 200, msg = "请求成功", data });
		}
	}
}
