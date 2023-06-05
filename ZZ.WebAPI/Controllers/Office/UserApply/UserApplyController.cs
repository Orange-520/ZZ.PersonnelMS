using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ZZ.Commons;
using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.Office;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.Office.UserApply
{
	[Route("Office/[controller]/[action]")]
	[ApiController]
	//[Authorize]
	public class UserApplyController : ControllerBase
	{
		private readonly MyDbContext db;
		private readonly CommonRepository commonRepository;
		private readonly IdentityRepository identityRepository;

		public UserApplyController(MyDbContext db, CommonRepository commonRepository, IdentityRepository identityRepository)
		{
			this.db = db;
			this.commonRepository = commonRepository;
			this.identityRepository = identityRepository;
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
			// 获取申请人的 Guid
			string guid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			User user = await this.identityRepository.GetByUserIdAsync(guid);
			if (user == null)
			{
				return StatusCode(400, new { code = 400, msg = "发送此请求的用户不存在" });
			}

			Department d = await this.commonRepository.FindByDepartmentIdAsync(hReq.DepartmentId);
			if (d == null)
			{
				return StatusCode(400, new { code = 400, msg = "部门不存在" });
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

			HiringNeedApply h = new HiringNeedApply(user, hReq.Content, null, d, p, hReq.NeedPersonCount);
			this.db.HiringNeedApplys.Add(h);

			return StatusCode(200, new { code = 200, msg = "申请招聘需求成功，请等待审核" });
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
			// 获取申请人的 Guid
			string guid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			User user = await this.identityRepository.GetByUserIdAsync(guid);
			if (user == null)
			{
				return StatusCode(400, new { code = 400, msg = "发送此请求的用户不存在" });
			}

			// 获取审核人的Guid
			User checkUser = await this.identityRepository.GetByUserIdAsync(req.CheckUser);
			if (checkUser == null)
			{
				return StatusCode(400, new { code = 400, msg = "审核人不存在" });
			}

			// 获取int值对应的枚举类型
			AskForLeaveType askForLeaveType = ForeachEnum.GetEnumFromInt<AskForLeaveType>(req.AskForLeaveType);
			if (askForLeaveType == AskForLeaveType.None)
			{
				return StatusCode(400, new { code = 400, msg = "请假类型错误" });
			}

			// 获取int值对应的枚举类型
			ApplyType applyType = ForeachEnum.GetEnumFromInt<ApplyType>(req.ApplyType);
			if (applyType != ApplyType.AskForLeave)
			{
				return StatusCode(400, new { code = 400, msg = "申请类型错误" });
			}

			AskForLeaveApply a = new AskForLeaveApply(user, req.Content, checkUser, req.HowLong, askForLeaveType, req.StartTime, req.EndTime);

			this.db.AskForLeaveApplys.Add(a);

			return StatusCode(200, new { code = 200, msg = "添加请假单成功" });
		}

		/// <summary>
		/// 获取用户所有的申请
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> GetAllUserApply(Paging req)
		{
			// 获取此次请求的用户Id
			Claim? claim = this.User.Claims.SingleOrDefault(e=>e.Type == ClaimTypes.NameIdentifier);
			if (claim == null) 
			{
				return StatusCode(400, new { code = 400, msg = "请携带正确的token" });
			}

			User user = await this.identityRepository.GetByUserIdAsync(claim.Value);
			if (user == null)
			{
				return StatusCode(400, new { code = 400, msg = "发送此请求的用户不存在" });
			}

			// 准备一个容器
			List<UserApplyResponse> data = new List<UserApplyResponse>();

			// 内存中加载
			IQueryable<UserApplyResponse> sql1 = this.db.AskForLeaveApplys.Select(e => new UserApplyResponse
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

			IQueryable<UserApplyResponse> sql2 = this.db.HiringNeedApplys.Select(e => new UserApplyResponse
			{
				Id = e.Id,
				CheckUser = e.CheckUser,
				CreateTime = e.CreationTime,
				ApplyType = e.ApplyType,
				Content = e.Content,
				CheckState = e.CheckState,
			});
			foreach (UserApplyResponse item in sql2)
			{
				data.Add(item);
			}

			int count = data.Count;
			data = data.Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToList();

			return StatusCode(200, new HttpResult(200,"请求成功", count, data));
		}

		[HttpGet]
		[Authorize]
		public async Task<string> Test()
		{
			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			string token = HttpContext.Request.Headers["Authorization"].ToString().Substring(7); // 从请求头中获取JWT令牌
			var jwtToken = (JwtSecurityToken)handler.ReadToken(token);
			Claim claim = jwtToken.Claims.SingleOrDefault(e => e.Type == ClaimTypes.NameIdentifier);
			if (claim == null)
			{
				return "token401";
			}
			string id = claim.Value;
			return id;

			//var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value; // 获取userId声明的值

			//string userGuid = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			//User user = await this.identityRepository.GetByUserAsync(userGuid);
		}

		[HttpGet]
		public async Task<IActionResult> Test2()
		{
			// 不行，
			Claim id = this.User.Claims.SingleOrDefault(e => e.Type == ClaimTypes.NameIdentifier);
			Claim userName = this.User.Claims.SingleOrDefault(e => e.Type == ClaimTypes.Name);
			List<Claim> roles = this.User.Claims.Where(e => e.Type == ClaimTypes.Role).ToList();


			// 不行
			//Claim id = this.User.FindFirst(ClaimTypes.NameIdentifier);
			//Claim userName = this.User.FindFirst(ClaimTypes.Name);
			//Claim roles = this.User.FindFirst(ClaimTypes.Role);
			//bool IsRoles = this.User.IsInRole("Admin");

			//JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			//string token = HttpContext.Request.Headers["Authorization"].ToString().Substring(7); // 从请求头中获取JWT令牌
			//JwtSecurityToken jwtToken = (JwtSecurityToken)handler.ReadToken(token);
			//Claim id = jwtToken.Claims.SingleOrDefault(e => e.Type == ClaimTypes.NameIdentifier);
			//Claim userName = jwtToken.Claims.SingleOrDefault(e => e.Type == ClaimTypes.Name);
			//List<Claim> roles = jwtToken.Claims.Where(e => e.Type == ClaimTypes.Role).ToList();

			return StatusCode(200, new
			{
				id= id.Value,
				name= userName.Value,
				roles= roles
			});

			//var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value; // 获取userId声明的值

			//string userGuid = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			//User user = await this.identityRepository.GetByUserAsync(userGuid);
		}
	}
}
