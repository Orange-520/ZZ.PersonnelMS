using Microsoft.AspNetCore.Mvc;
using ZZ.Commons;
using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.JoinUs;
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

		public JoinUsController(MyDbContext db, CommonRepository commonRepository)
		{
			this.db = db;
			this.commonRepository = commonRepository;
		}

		/// <summary>
		/// 添加简历
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> AddResume(ResumeRequest req)
		{
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

			Resume r = ForeachProperty.TransReflection<ResumeRequest,Resume>(req);
			Task t1 = Task.Run(() => {
				foreach (var item in req.MinWorkHistory)
				{
					WorkHistory w = ForeachProperty.TransReflection<MinWorkHistory, WorkHistory>(item);
					r.WorkHistory.Add(w);
				}
				return Task.CompletedTask;
			});
			Task t2 = Task.Run(() => {
				foreach (var item in req.MinEducationHistory)
				{
					EducationHistory e = ForeachProperty.TransReflection<MinEducationHistory, EducationHistory>(item);
					r.EducationHistory.Add(e);
				}
			});
			Task t3 = Task.Run(() => {
				foreach (var item in req.MinCertificate)
				{
					Certificate c = ForeachProperty.TransReflection<MinCertificate, Certificate>(item);
					r.Certificate.Add(c);
				}
			});
			await Task.WhenAll(t1,t2,t3);
			r.Department = d;
			r.Position = p;
			this.db.Resumes.Add(r);
			return StatusCode(200,new { code =200 , msg = "添加成功"});
		}

		[HttpPost]
		public Resume Test(ResumeRequest req)
		{
			Resume r = ForeachProperty.TransReflection<ResumeRequest, Resume>(req);
			return r;
		}
	}

}
