using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZZ.Commons;
using ZZ.Domain.Entities.JoinUs;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.JoinUs.Resume
{
    [Route("JoinUs/[controller]/[action]")]
	[ApiController]
    [Authorize]
	public class ResumeController : ControllerBase
	{
		private readonly MyDbContext db;
		private readonly IdentityRepository identityRepository;
        private readonly DepartmentRepository departmentRepository;
        private readonly PositionRepository positionRepository;
        private readonly OfficeRepository officeRepository;
        private readonly JoinUsRepository joinUsRepository;

        public ResumeController(MyDbContext db, IdentityRepository identityRepository, DepartmentRepository departmentRepository, PositionRepository positionRepository, OfficeRepository officeRepository, JoinUsRepository joinUsRepository)
        {
            this.db = db;
            this.identityRepository = identityRepository;
            this.departmentRepository = departmentRepository;
            this.positionRepository = positionRepository;
            this.officeRepository = officeRepository;
            this.joinUsRepository = joinUsRepository;
        }

        /// <summary>
        /// 添加简历
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> AddResume(AddResumeRequest req)
        {
            // 获取此次请求的用户Id
            string checkUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 判断审核的用户是否存在
            var findCheckUserResult = await this.identityRepository.GetByUserIdAsync(checkUserId);
            if (!findCheckUserResult.Item1)
            {
                return BadRequest(new ApiResponseBase(400, "发送此次请求的用户不存在"));
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

            // 中转实体变量h
            ZZ.Domain.Entities.Office.HiringNeedApply? h = null;
            // 判断是否要关联招聘需求
            if (req.HiringNeedApplyId != null)
            {
                var findHiringNeedApplyResult = await this.officeRepository.FindHiringNeedApplyByIdAsync(req.HiringNeedApplyId);
                if (!findHiringNeedApplyResult.Item1)
                {
                    return BadRequest(new ApiResponseBase(400, findCheckUserResult.Item2));
                }
                // 中转实体变量赋值h
                h = findHiringNeedApplyResult.Item3;
            }

            /* 对象数据映射 */
            // 一个对象的部分属性映射到另一个实体，生成简历实体
            ZZ.Domain.Entities.JoinUs.Resume r = ForeachProperty.TransReflection<AddResumeRequest, ZZ.Domain.Entities.JoinUs.Resume>(req);

            // 任务一
            Task t1 = Task.Run(() => {
                foreach (var item in req.MinWorkHistory)
                {
                    // 一个对象的部分属性映射到另一个实体，生成工作经历实体
                    WorkHistory w = ForeachProperty.TransReflection<MinWorkHistory, WorkHistory>(item);
                    r.WorkHistory.Add(w);
                }
                return Task.CompletedTask;
            });
            // 任务二
            Task t2 = Task.Run(() => {
                foreach (var item in req.MinEducationHistory)
                {
                    // 一个对象的部分属性映射到另一个实体，生成教育经历实体
                    EducationHistory e = ForeachProperty.TransReflection<MinEducationHistory, EducationHistory>(item);
                    r.EducationHistory.Add(e);
                }
            });
            // 任务三
            Task t3 = Task.Run(() => {
                foreach (var item in req.MinCertificate)
                {
                    // 一个对象的部分属性映射到另一个实体，生成获奖经历实体
                    Certificate c = ForeachProperty.TransReflection<MinCertificate, Certificate>(item);
                    r.Certificate.Add(c);
                }
            });
            await Task.WhenAll(t1, t2, t3);
            r.Department = d.Item3;
            r.Position = p.Item3;
            r.CheckUser = findCheckUserResult.Item3;

            // 如果招聘需求不为空，则将简历关联招聘需求。
            if (h != null)
            {
                r.HiringNeedApply = h;
            }

            this.db.Resumes.Add(r);
            return Ok(new ApiResponseBase(200, "添加成功"));
        }

        /// <summary>
        /// 获取应聘列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
		public IActionResult GetResumeList(ResumeListRequest req)
		{
			IQueryable<ZZ.Domain.Entities.JoinUs.Resume> sql = this.db.Resumes.Include(e => e.Department).Include(e => e.Position).Include(e=>e.WorkHistory).Include(e => e.EducationHistory).Include(e => e.Certificate);

			// 是否搜索职位名称关键字
			if (req.NameKey != null && req.NameKey != "")
			{
				sql = sql.Where(e=> e.Name.Contains(req.NameKey));
			}

			JoinUsResult j = ForeachEnum.GetEnumFromInt<JoinUsResult>(req.JoinUsResult);

			// 如果简历还没有结果，就是未处理完的状态，则关联查询当前处理人
			if (j == JoinUsResult.None)
			{
				sql = sql.Include(e => e.CheckUser);
			}

			// 根据应聘结果显示不同的简历
			sql = sql.Where(e => e.JoinUsResult == j);

			List<ZZ.Domain.Entities.JoinUs.Resume> data = sql.Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToList();

			int total = sql.Count();

			return Ok(new ApiResponseChildB(200,"获取成功",total,data));
			
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
            string checkUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 判断审核的用户是否存在
            var findCheckUserResult = await this.identityRepository.GetByUserIdAsync(checkUserId);
            if (!findCheckUserResult.Item1)
            {
                return BadRequest(new ApiResponseBase(400, "发送此次请求的用户不存在"));
            }

            // 判断此简历是否存在
            var findResumeResult = await this.joinUsRepository.FindResumeByIdAsync(req.ResumeId);
			if (!findResumeResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400,findCheckUserResult.Item2));
			}

			// 判断处理环节是否错误
			JoinUsStep s = ForeachEnum.GetEnumFromInt<JoinUsStep>(req.JoinUsStep);
			if (s == JoinUsStep.None)
			{
				return BadRequest(new ApiResponseBase(400, "此招聘环节不存在"));
			}

			// 改变实体状态
			findResumeResult.Item3.JoinUsStep= s;
            findResumeResult.Item3.CheckUser= findCheckUserResult.Item3;

			return Ok(new ApiResponseBase(200, "处理成功！"));
		}

		/// <summary>
		/// 处理简历的结果
		/// </summary>
		[HttpPost]
		[UnitOfWork(typeof(MyDbContext))]
		public async Task<IActionResult> ChangeJoinUsResult(ChangeJoinUsResultRequest req)
		{
            // 获取此次请求的用户Id
            string checkUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 判断审核的用户是否存在
            var findCheckUserResult = await this.identityRepository.GetByUserIdAsync(checkUserId);
            if (!findCheckUserResult.Item1)
            {
                return BadRequest(new ApiResponseBase(400, "发送此次请求的用户不存在"));
            }

            // 判断此简历是否存在
            var findResumeResult = await this.joinUsRepository.FindResumeByIdPlusAsync(req.ResumeId);
            if (!findResumeResult.Item1)
            {
                return BadRequest(new ApiResponseBase(400,findResumeResult.Item2));
            }

			// 判断是否决定了简历结果
			JoinUsResult j = ForeachEnum.GetEnumFromInt<JoinUsResult>(req.JoinUsResult);
			if (j == JoinUsResult.None)
			{
                return BadRequest(new ApiResponseBase(400, "处理错误"));
			}
			else if (j == JoinUsResult.入职)
			{
				// 如果此简历关联了招聘需求
				if (findResumeResult.Item3.HiringNeedApply != null)
				{
					// 入职了才让招聘需求中的已招聘人数自增
					findResumeResult.Item3.HiringNeedApply.HasPersonCount++;
				}
			}
            // 决定简历的结果。
            findResumeResult.Item3.JoinUsResult = j;
            // 最后一次的处理人。
            findResumeResult.Item3.CheckUser= findCheckUserResult.Item3;

			return Ok(new ApiResponseBase(200, "处理成功！"));
		}
	}
}
