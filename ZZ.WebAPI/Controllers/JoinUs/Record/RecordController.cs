using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZZ.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.JoinUs;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.JoinUs.Record
{
    [Route("JoinUs/[controller]/[action]")]
	[ApiController]
    [Authorize]
	public class RecordController : ControllerBase
	{
		private readonly MyDbContext db;
        private readonly IdentityRepository identityRepository;
        private readonly DepartmentRepository departmentRepository;
        private readonly PositionRepository positionRepository;

        public RecordController(MyDbContext db, IdentityRepository identityRepository, DepartmentRepository departmentRepository, PositionRepository positionRepository)
        {
            this.db = db;
            this.identityRepository = identityRepository;
            this.departmentRepository = departmentRepository;
            this.positionRepository = positionRepository;
        }
        /// <summary>
        /// 添加档案
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> AddRecord(AddRecordRequest req)
        {
            // 获取此次请求的用户Id
            string checkUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 判断审核的用户是否存在
            var findCheckUserResult = await this.identityRepository.GetByUserIdAsync(checkUserId);
            if (!findCheckUserResult.Item1)
            {
                return BadRequest(new ApiResponseBase(400, "发送此次请求的用户不存在"));
            }

            // 判断用户名是否存在
            var findUserResult = await this.identityRepository.FindByUserNameAsync(req.UserName);
            if (findUserResult.Item1)
            {
                return BadRequest(new ApiResponseBase(400, "用户名已存在"));
            }

            // 创建用户
            User user = new(req.UserName) { NickName = req.Name };
            var createUserResult = await this.identityRepository.CreateUserAsync(user, req.Password);
            if (!createUserResult.Item1)
            {
                return BadRequest(new ApiResponseBase(400, createUserResult.Item2));
            }

            // 判断部门是否存在
            var d = await this.departmentRepository.FindDepartmentByIdAsync(req.DepartmentId);
            if (!d.Item1)
            {
                return BadRequest(new ApiResponseBase(400, d.Item2));
            }

            // 判断职位是否存在
            var p = await this.positionRepository.FindPositionByIdAsync(req.PositionId);
            if (!p.Item1)
            {
                return BadRequest(new ApiResponseBase(400, p.Item2));
            }

            // 用工性质
            NatureOfEmployment natureOfEmployment = ForeachEnum.GetEnumFromInt<NatureOfEmployment>(req.NatureOfEmployment);
            if (natureOfEmployment == NatureOfEmployment.None)
            {
                return BadRequest(new ApiResponseBase(400, "用工性质错误"));
            }

            // 婚姻状况
            MaritalStatus maritalStatus = ForeachEnum.GetEnumFromInt<MaritalStatus>(req.MaritalStatus);
            if (maritalStatus == MaritalStatus.None)
            {
                return BadRequest(new ApiResponseBase(400, "婚姻状况错误"));
            }

            PoliticsStatus politicsStatus = PoliticsStatus.None;
            // 政治面貌
            if (req.PoliticsStatus != null && req.PoliticsStatus != 0)
            {
                politicsStatus = ForeachEnum.GetEnumFromInt<PoliticsStatus>(req.PoliticsStatus);
                if (politicsStatus == PoliticsStatus.None)
                {
                    return BadRequest(new ApiResponseBase(400, "政治面貌错误"));
                }
            }

            // 当前学历
            CurrentEducation currentEducation = ForeachEnum.GetEnumFromInt<CurrentEducation>(req.CurrentEducation);
            if (currentEducation == CurrentEducation.None)
            {
                return BadRequest(new ApiResponseBase(400, "当前学历错误"));
            }

            // 求职方式
            JobHuntingMode jobHuntingMode = ForeachEnum.GetEnumFromInt<JobHuntingMode>(req.JobHuntingMode);
            if (jobHuntingMode == JobHuntingMode.None)
            {
                return BadRequest(new ApiResponseBase(400, "求职方式错误"));
            }

            // 对象数据映射
            ZZ.Domain.Entities.JoinUs.Record r = ForeachProperty.TransReflection<AddRecordRequest, ZZ.Domain.Entities.JoinUs.Record>(req);

            // 任务一
            Task t1 = Task.Run(() => {
                foreach (var item in req.MinWorkHistory)
                {
                    // 取出属性，生成工作经历实体
                    WorkHistory w = ForeachProperty.TransReflection<MinWorkHistory, WorkHistory>(item);
                    r.WorkHistory.Add(w);
                }
                return Task.CompletedTask;
            });
            // 任务二
            Task t2 = Task.Run(() => {
                foreach (var item in req.MinEducationHistory)
                {
                    // 取出属性，生成教育经历实体
                    EducationHistory e = ForeachProperty.TransReflection<MinEducationHistory, EducationHistory>(item);
                    r.EducationHistory.Add(e);
                }
            });
            // 任务三
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
            r.CheckUser = findCheckUserResult.Item3;
            r.Department = d.Item3;
            r.Position = p.Item3;

            this.db.Records.Add(r);

            return Ok(new ApiResponseBase(200, "添加成功"));
        }
        

        /// <summary>
        /// 获取档案列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
		public IActionResult GetRecordList(RecordListRequest req)
		{
			IQueryable<ZZ.Domain.Entities.JoinUs.Record> sql = this.db.Records.Include(e => e.User).Include(e=>e.Department).Include(e=>e.Position);

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

			return Ok(new ApiResponseChildB(200,"获取成功",total, data));
		}
	}
}
