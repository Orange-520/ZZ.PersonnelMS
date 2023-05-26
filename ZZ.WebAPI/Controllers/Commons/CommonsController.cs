using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using ZZ.Domain.Entities.Commons;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.Commons
{
	[Route("[controller]/[action]")]
	[ApiController]
	//[Authorize]
	public class CommonsController : ControllerBase
	{
		private readonly CommonRepository repository;
		private readonly MyDbContext dbContext;

		public CommonsController(CommonRepository repository, MyDbContext dbContext)
		{
			this.repository = repository;
			this.dbContext = dbContext;
		}

		/// <summary>
		/// 添加部门
		/// </summary>
		/// <param name="dReq"></param>
		/// <returns></returns>
		[HttpPost]
		[UnitOfWork(typeof(MyDbContext))]
		public async Task<IActionResult> AddDepartment(DepartmentRequest dReq)
		{
			// 只能存在一个根部门。
			Department d;
			if (dReq.ParentDepartmenId == null)
			{
				// 查找上级部门为null的部门
				Department? result = await this.dbContext.Departments.SingleOrDefaultAsync(e=>e.ParentDepartmen == null);
				// 找不到的话，创建此部门
				if (result == null)
				{
					d = new Department()
					{
						Name = dReq.DepartmentName,
						Description = dReq.Description,
						ParentDepartmen = result
					};
				}
				else
				{
					return StatusCode(400, new { code = 400, msg = "已存在总部门" });
				}
			}
			// 如果上级部门不为null
			else
			{
				// 寻找此上级部门是否存在
				Department? result = await this.repository.FindByDepartmentIdAsync(dReq.ParentDepartmenId);
				if (result == null)
				{
					return StatusCode(400, new { code = 400, msg = "该部门不存在" });
				}
				else
				{
					d = new Department()
					{
						Name = dReq.DepartmentName,
						Description = dReq.Description,
						ParentDepartmen = result
					};
				}
			}
			this.dbContext.Departments.Add(d);

			return StatusCode(200, new { code = 200, msg = "添加成功" });
		}

		/// <summary>
		/// 添加职位
		/// </summary>
		/// <param name="pReq"></param>
		/// <returns></returns>
		[HttpPost]
		[UnitOfWork(typeof(MyDbContext))]
		public async Task<IActionResult> AddPosition(PositionRequest pReq)
		{
			(bool result,string msg,Department? d) = await this.repository.FindPositionNameByDepartment(pReq.PositionName, pReq.DepartmentId);
			if (!result)
			{
				return StatusCode(400,new { code = 400,msg });
			}

			Position? p = new () 
			{ 
				Name= pReq.PositionName,
				Description= pReq.Description
			};
			p.Department = d!;

			this.dbContext.Positions.Add(p);
			return StatusCode(200, new { code = 200, msg = "添加职位成功" });
		}

		/// <summary>
		/// 获取所有部门
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetAllDepartment()
		{
			Department? d = await this.dbContext.Departments.FirstOrDefaultAsync(e => e.ParentDepartmen == null);
			if (d == null)
			{
				return StatusCode(200, new { msg = "请设立根部门", data = new ArrayList() });
			}
			d.ChildrenDepartment = this.repository.GetAllDepartment(d!);
			return StatusCode(200, new { msg = "部门列表", data = d });
		}

		/// <summary>
		/// 获取某个部门下的职位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> FindPositionByDepartment([FromQuery(Name ="id")]int id)
		{
			Department? d = await this.repository.FindByDepartmentIdAsync(id);
			if (d == null)
			{
				return StatusCode(400, new { code = 400, msg = "部门不存在" });
			}
			dynamic p =  this.repository.FindPositionByDepartment(d);
			return StatusCode(200, new { code = 200, msg = "获取成功", data = p });
		}
	}
}
