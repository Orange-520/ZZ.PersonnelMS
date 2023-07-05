using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZZ.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.Office;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.Office.UserApply
{
	[Route("Office/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class UserApplyController : ControllerBase
	{
		private readonly MyDbContext db;
		private readonly IdentityRepository identityRepository;
		private readonly DepartmentRepository departmentRepository;
		private readonly PositionRepository positionRepository;

        public UserApplyController(MyDbContext db, IdentityRepository identityRepository, DepartmentRepository departmentRepository, PositionRepository positionRepository)
        {
            this.db = db;
            this.identityRepository = identityRepository;
            this.departmentRepository = departmentRepository;
            this.positionRepository = positionRepository;
        }

        /// <summary>
        /// 添加招聘需求申请
        /// </summary>
        /// <param name="hReq"></param>
        /// <returns></returns>
        [HttpPost]
		[UnitOfWork(typeof(MyDbContext))]
		public async Task<IActionResult> AddHiringNeedApply(HiringNeedApplyRequest hReq)
		{
            if (hReq.NeedPersonCount < 0)
            {
                return BadRequest(new ApiResponseBase(400, "招聘人数应该大于零"));
            }

            // 获取int值对应的枚举类型
            ApplyType applyType = ForeachEnum.GetEnumFromInt<ApplyType>(hReq.ApplyType);
            if (applyType != ApplyType.HiringNeeds)
            {
                return BadRequest(new ApiResponseBase(400, "申请类型错误"));
            }

            // 获取申请人的 Guid
            string guid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var findUserResult = await this.identityRepository.GetByUserIdAsync(guid);
			if (!findUserResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400, "发送此请求的用户不存在"));
			}

			// 查找部门是否存在
			var findDepartmentResult = await this.departmentRepository.FindDepartmentByIdAsync(hReq.DepartmentId);
			if (!findDepartmentResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400, findDepartmentResult.Item2));
			}

			// 查找职位是否存在
			var findPositionResult = await this.positionRepository.FindPositionByIdAsync(hReq.PositionId);
			if (!findPositionResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400, findPositionResult.Item2));
			}

			// 创建招聘需求实体
			HiringNeedApply h = new HiringNeedApply(findUserResult.Item3, hReq.Content, null, findDepartmentResult.Item3, findPositionResult.Item3, hReq.NeedPersonCount);
			this.db.HiringNeedApplys.Add(h);

			return Ok(new ApiResponseBase(200, "申请招聘需求成功，请等待审核"));
		}

		/// <summary>
		/// 添加请假单
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost]
		[UnitOfWork(typeof(MyDbContext))]
		public async Task<IActionResult> AddAskForLeaveApply(AskForLeaveApplyRequest req)
		{
            // 获取int值对应的枚举类型
            AskForLeaveType askForLeaveType = ForeachEnum.GetEnumFromInt<AskForLeaveType>(req.AskForLeaveType);
            if (askForLeaveType == AskForLeaveType.None)
            {
                return BadRequest(new ApiResponseBase(400, "请假类型错误"));
            }

            // 获取int值对应的枚举类型
            ApplyType applyType = ForeachEnum.GetEnumFromInt<ApplyType>(req.ApplyType);
            if (applyType != ApplyType.AskForLeave)
            {
                return BadRequest(new ApiResponseBase(400, "申请类型错误"));
            }

            // 获取申请人的 Guid
            string guid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var findUserResult = await this.identityRepository.GetByUserIdAsync(guid);
            if (!findUserResult.Item1)
            {
                return BadRequest(new ApiResponseBase(400, "发送此请求的用户不存在"));
            }

            // 获取审核人的Guid
            var findCheckUserResult = await this.identityRepository.GetByUserIdAsync(req.CheckUser);
			if (!findCheckUserResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400, "审核人不存在"));
			}
			
			// 创建请假单实体
			AskForLeaveApply a = new AskForLeaveApply(findUserResult.Item3, req.Content, findCheckUserResult.Item3, req.HowLong, askForLeaveType, req.StartTime, req.EndTime);
			this.db.AskForLeaveApplys.Add(a);

			return Ok(new ApiResponseBase(200, "添加请假单成功"));
		}

		/// <summary>
		/// 获取用户所有的申请
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public IActionResult GetAllUserApply(PagingRequestBase req)
		{
			// 获取此次请求中，用户的Id
			string guidStr = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
			Guid userId = Guid.Parse(guidStr);

			// 准备一个容器
			List<UserApplyResponse> data = new List<UserApplyResponse>();

			// 获取用户的所有请假单
			IQueryable<UserApplyResponse> sql1 = this.db.AskForLeaveApplys.Where(e=>e.User.Id == userId).Select(e => new UserApplyResponse
			{
				Id = e.Id,
				CheckUser = e.CheckUser,
				CreateTime = e.CreationTime,
				ApplyType = e.ApplyType,
				Content = e.Content,
				CheckState = e.CheckState,
			});

			// 获取用户的所有招聘需求
			IQueryable<UserApplyResponse> sql2 = this.db.HiringNeedApplys.Where(e => e.User.Id == userId).Select(e => new UserApplyResponse
			{
				Id = e.Id,
				CheckUser = e.CheckUser,
				CreateTime = e.CreationTime,
				ApplyType = e.ApplyType,
				Content = e.Content,
				CheckState = e.CheckState,
			});

			foreach (UserApplyResponse item in sql1)
			{
				data.Add(item);
			}
			foreach (UserApplyResponse item in sql2)
			{
				data.Add(item);
			}

			int count = data.Count;
			data = data.Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToList();

			return Ok(new ApiResponseChildB(200,"请求成功", count, data));
		}
	}
}
