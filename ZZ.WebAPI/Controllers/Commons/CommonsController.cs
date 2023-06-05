using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using ZZ.Commons;
using ZZ.Domain.Entities.Commons;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.Commons
{
	[Route("[controller]/[action]")]
	[ApiController]
	public class CommonsController : ControllerBase
	{
		private readonly CommonRepository repository;
		private readonly MyDbContext dbContext;
		private readonly IConfiguration config;

		public CommonsController(CommonRepository repository, MyDbContext dbContext, IConfiguration config)
		{
			this.repository = repository;
			this.dbContext = dbContext;
			this.config = config;
		}

		/// <summary>
		/// 添加部门
		/// </summary>
		/// <param name="dReq"></param>
		/// <returns></returns>
		[HttpPost]
		[UnitOfWork(typeof(MyDbContext))]
		[Authorize(Roles = RoleType.A)]
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
		[Authorize(Roles = RoleType.A)]
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
		[Authorize]
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
		[Authorize]
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

		/// <summary>
		/// 上传文件，通过参数的形式接收文件流
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Upload(IFormFile file)
		{
			if (file != null && file.Length > 0)
			{
				string configFilePath = this.config["FilePath"];
				if (configFilePath == null)
				{
					return StatusCode(400, new {code = 400,msg= "服务端未配置文件保存路径" });
				}

				// 创建一个二进制数组，长度为文件长度
				byte[] data = new byte[file.Length];
				// 这段using的作用是什么
				using (var stream = new MemoryStream(data))
				{
					await file.CopyToAsync(stream);
				}

				// 保存处理后的图片数据到指定目录，以 GUID 命名防止文件名冲突
				var fileName = Guid.NewGuid().ToString() + ".jpg";
				// 将多个字符串拼接成路径
				var filePath = Path.Combine(configFilePath, fileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await stream.WriteAsync(data);
				}
				// 返回保存的文件名
				return Ok(new { Code = 200,FilePath = fileName });
			}
			return BadRequest(new {Code = 400,msg = "无文件"});
		}

		/// <summary>
		/// 文件上传方式二，在 HttpContext 中获取文件流
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Upload2()
		{
			string configFilePath = this.config["FilePath"];
			if (configFilePath == null)
			{
				return StatusCode(400, new { code = 400, msg = "服务端未配置文件保存路径" });
			}
			IFormFile file = this.HttpContext.Request.Form.Files[0];
			//var file = Request.Form.Files[0];
			if (file != null && file.Length > 0)
			{
				var fileName = $"{Guid.NewGuid()}-{file.FileName}";
				// 将多个字符串拼接成路径。
				var filePath = Path.Combine(configFilePath, fileName);
				using var stream = new FileStream(filePath, FileMode.Create);
				await file.CopyToAsync(stream);
				return Ok(new { Code = 200, FilePath = fileName });
			}
			return BadRequest(new { Code = 400, msg = "无文件" });
		}

		[HttpPost]
		[NonAction]
		public string Test(IFormFile file)
		{
			//var file = this.HttpContext.Request.Form.Files[0];
			// 计算文件的哈希值
			string hashValue = HashHelper.ComputeSha256Hash(file.OpenReadStream());
			return hashValue;
		}
		
	}
}
