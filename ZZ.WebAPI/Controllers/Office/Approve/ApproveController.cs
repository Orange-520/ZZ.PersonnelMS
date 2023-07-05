using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZZ.Commons;
using ZZ.Domain.Entities.Office;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.Office.Approve
{
	[Route("Office/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class ApproveController : ControllerBase
	{
		private readonly MyDbContext db;
		private readonly IdentityRepository identityRepository;
		private readonly OfficeRepository officeRepository;

		public ApproveController(MyDbContext db, IdentityRepository identityRepository, OfficeRepository officeRepository)
		{
			this.db = db;
			this.identityRepository = identityRepository;
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

			// 获取所有请假单申请
			IQueryable<ApplyResponse> askForLeaveApplyList = this.db.AskForLeaveApplys.Where(e => e.CheckState == CheckState.Wait).Select(e => new ApplyResponse
			{
				Id = e.Id,
				User = e.User,
				CreateTime = e.CreationTime,
				ApplyType = e.ApplyType,
				Content = e.Content
			});
			foreach (ApplyResponse item in askForLeaveApplyList)
			{
				data.Add(item);
			}

			// 获取所有招聘需求申请
			IQueryable<ApplyResponse> hiringNeedApplyList = this.db.HiringNeedApplys.Where(e => e.CheckState == CheckState.Wait).Select(e => new ApplyResponse
			{
				Id = e.Id,
				User = e.User,
				CreateTime = e.CreationTime,
				ApplyType = e.ApplyType,
				Content = e.Content
			});
			foreach (ApplyResponse item in hiringNeedApplyList)
			{
				data.Add(item);
			}

			//data.Skip((page.PageIndex - 1) * page.PageSize).Take(page.PageSize);

			return Ok(new ApiResponseChildA(200, "请求成功",data));
		}
		
		/// <summary>
		/// 审核申请
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[UnitOfWork(typeof(MyDbContext))]
		public async Task<IActionResult> CheckApply(CheckApplyRequest req)
		{
            // 获取此次请求的用户Id
            string checkUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 判断审核的用户是否存在
            var findCheckUserResult = await this.identityRepository.GetByUserIdAsync(checkUserId);
            if (!findCheckUserResult.Item1)
            {
                return BadRequest(new ApiResponseBase(400, "发送此次请求的用户不存在"));
            }

            // 检查申请类型是否正确
            ApplyType applyType = ForeachEnum.GetEnumFromInt<ApplyType>(req.ApplyType);
			if (applyType == ApplyType.None)
			{
				return BadRequest(new ApiResponseBase(400, "申请类型错误"));
			}

			// 检查审核类型是否正确
			CheckState checkState = ForeachEnum.GetEnumFromInt<CheckState>(req.CheckState);
			if (checkState == CheckState.None)
			{
				return BadRequest(new ApiResponseBase(400, "审核类型错误"));
			}

			// 获取申请实体
			var findApplyResult = await this.officeRepository.FindApplyByIdAsync(applyType, req.Id);
			if (!findApplyResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400,findApplyResult.Item2));
			}
			// 修改实体状态
			findApplyResult.Item3.AlterCheckState(checkState);
            findApplyResult.Item3.CheckUser = findCheckUserResult.Item3;
            findApplyResult.Item3.ListModificationTime = DateTime.Now;

			// 回复表
			Reply r = new(findApplyResult.Item3.User, findCheckUserResult.Item3, ReplyType.个人申请, req.ReplyTitle, req.ReplyContent);
			this.db.Replys.Add(r);
			return Ok(new ApiResponseBase(200, "审核成功"));
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
				return BadRequest(new ApiResponseBase(400, "申请类型错误"));
			}

			// 申请详情
			var findApplyResult = await this.officeRepository.FindApplyByIdAsync(applyType, req.Id);
			if (!findApplyResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400,findApplyResult.Item2));
			}

			return Ok(new ApiResponseChildA(200, "请求成功",findApplyResult.Item3));
		}
	}
}
