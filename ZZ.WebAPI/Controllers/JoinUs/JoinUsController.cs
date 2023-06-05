using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZZ.Commons;
using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.JoinUs;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.JoinUs
{
    [Route("[controller]/[action]")]
	[ApiController]
	[UnitOfWork(typeof(MyDbContext))]
	public class JoinUsController : ControllerBase
	{
		private readonly MyDbContext db;
		private readonly CommonRepository commonRepository;
		private readonly IdentityRepository identityRepository;

		public JoinUsController(MyDbContext db, CommonRepository commonRepository, IdentityRepository identityRepository)
		{
			this.db = db;
			this.commonRepository = commonRepository;
			this.identityRepository = identityRepository;
		}

		/// <summary>
		/// 添加简历
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> AddResume(AddResumeRequest req)
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

			Department? d = await this.commonRepository.FindByDepartmentIdAsync(req.DepartmentId);
			if (d == null)
			{
				return StatusCode(400, new { code=400,msg = "部门不存在"});
			}
			
			Position? p = await this.commonRepository.FindPositionAsync(req.PositionId);
			if (p == null)
			{
				return StatusCode(400, new { code = 400, msg = "职位不存在" });
			}

			ZZ.Domain.Entities.Office.HiringNeedApply? h = null;
			// 判断是否要关联招聘需求
			if (req.HiringNeedApplyId != null)
			{
				h = await this.db.HiringNeedApplys.FindAsync(req.HiringNeedApplyId);

				if (h == null)
				{
					return StatusCode(400, new { code = 400, msg = "招聘需求不存在" });
				}
			}

			// 取出属性，生成简历实体
			ZZ.Domain.Entities.JoinUs.Resume r = ForeachProperty.TransReflection<AddResumeRequest, ZZ.Domain.Entities.JoinUs.Resume>(req);

			Task t1 = Task.Run(() => {
				foreach (var item in req.MinWorkHistory)
				{
					// 取出属性，生成工作经历实体
					WorkHistory w = ForeachProperty.TransReflection<MinWorkHistory, WorkHistory>(item);
					r.WorkHistory.Add(w);
				}
				return Task.CompletedTask;
			});
			Task t2 = Task.Run(() => {
				foreach (var item in req.MinEducationHistory)
				{
					// 取出属性，生成教育经历实体
					EducationHistory e = ForeachProperty.TransReflection<MinEducationHistory, EducationHistory>(item);
					r.EducationHistory.Add(e);
				}
			});
			Task t3 = Task.Run(() => {
				foreach (var item in req.MinCertificate)
				{
					// 取出属性，生成获奖经历实体
					Certificate c = ForeachProperty.TransReflection<MinCertificate, Certificate>(item);
					r.Certificate.Add(c);
				}
			});
			await Task.WhenAll(t1,t2,t3);
			r.Department = d;
			r.Position = p;
			r.CheckUser = checkUser;

			// 如果招聘需求不为空，则将简历关联招聘需求。
			if (h !=null)
			{
				r.HiringNeedApply = h;
			}

			this.db.Resumes.Add(r);
			return StatusCode(200,new { code =200 , msg = "添加成功"});
		}

		[HttpPost]
		public async Task<IActionResult> AddRecord(AddRecordRequest req)
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

			// 判断用户名是否存在
			if (await this.identityRepository.FindByUserNameAsync(req.UserName) != null) 
			{
				return StatusCode(400, new { code = 400, msg = "用户名已存在，请更换用户名" });
			}

			// 创建用户
			User user = new User(req.UserName) { NickName = req.Name };
			IdentityResult result = await this.identityRepository.CreateUserAsync(user,req.Password);
			if (!result.Succeeded) 
			{
				return StatusCode(400, new { code = 400, msg = "创建用户失败！" });
			}

			// 判断部门是否存在
			Department? d = await this.commonRepository.FindByDepartmentIdAsync(req.DepartmentId);
			if (d == null)
			{
				return StatusCode(400, new { code = 400, msg = "部门不存在" });
			}

			// 判断部门下的职位是否存在
			Position? p = await this.commonRepository.FindPositionAsync(req.PositionId);
			if (p == null)
			{
				return StatusCode(400, new { code = 400, msg = "职位不存在" });
			}

			// 用工性质
			NatureOfEmployment natureOfEmployment = ForeachEnum.GetEnumFromInt<NatureOfEmployment>(req.NatureOfEmployment);
			if (natureOfEmployment == NatureOfEmployment.None)
			{
				return StatusCode(400, new { code = 400, msg = "用工性质错误" });
			}

			// 婚姻状况
			MaritalStatus maritalStatus = ForeachEnum.GetEnumFromInt<MaritalStatus>(req.MaritalStatus);
			if (maritalStatus == MaritalStatus.None)
			{
				return StatusCode(400, new { code = 400, msg = "婚姻状况错误" });
			}

			PoliticsStatus politicsStatus = PoliticsStatus.None;
			// 政治面貌
			if (req.PoliticsStatus != null && req.PoliticsStatus != 0)
			{
				politicsStatus = ForeachEnum.GetEnumFromInt<PoliticsStatus>(req.PoliticsStatus);
				if (politicsStatus == PoliticsStatus.None)
				{
					return StatusCode(400, new { code = 400, msg = "政治面貌错误" });
				}
			}

			// 当前学历
			CurrentEducation currentEducation = ForeachEnum.GetEnumFromInt<CurrentEducation>(req.CurrentEducation);
			if (currentEducation == CurrentEducation.None)
			{
				return StatusCode(400, new { code = 400, msg = "当前学历错误" });
			}

			// 求职方式
			JobHuntingMode jobHuntingMode = ForeachEnum.GetEnumFromInt<JobHuntingMode>(req.JobHuntingMode);
			if (jobHuntingMode == JobHuntingMode.None)
			{
				return StatusCode(400, new { code = 400, msg = "求职方式错误" });
			}

			ZZ.Domain.Entities.JoinUs.Record r = ForeachProperty.TransReflection<AddRecordRequest, ZZ.Domain.Entities.JoinUs.Record>(req);

			Task t1 = Task.Run(() => {
				foreach (var item in req.MinWorkHistory)
				{
					// 取出属性，生成工作经历实体
					WorkHistory w = ForeachProperty.TransReflection<MinWorkHistory, WorkHistory>(item);
					r.WorkHistory.Add(w);
				}
				return Task.CompletedTask;
			});
			Task t2 = Task.Run(() => {
				foreach (var item in req.MinEducationHistory)
				{
					// 取出属性，生成教育经历实体
					EducationHistory e = ForeachProperty.TransReflection<MinEducationHistory, EducationHistory>(item);
					r.EducationHistory.Add(e);
				}
			});
			Task t3 = Task.Run(() => {
				foreach (var item in req.MinCertificate)
				{
					// 取出属性，生成获奖经历实体
					Certificate c = ForeachProperty.TransReflection<MinCertificate, Certificate>(item);
					r.Certificate.Add(c);
				}
			});
			await Task.WhenAll(t1, t2, t3);

			r.User = user;
			r.CheckUser = checkUser;
			r.Department = d;
			r.Position = p;

			this.db.Records.Add(r);

			return StatusCode(200,new {code = 200,msg ="添加成功"});
		}

	}

}
