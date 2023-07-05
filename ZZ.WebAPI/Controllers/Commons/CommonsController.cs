using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZZ.Commons;

namespace ZZ.WebAPI.Controllers.Commons
{
	[Route("[controller]/[action]")]
	[ApiController]
    [Authorize]
    public class CommonsController : ControllerBase
	{
		private readonly IConfiguration config;

		public CommonsController(IConfiguration config)
		{
			this.config = config;
		}

		/// <summary>
		/// 上传文件，通过参数的形式接收文件流
		/// </summary>
		/// <returns></returns>
		[HttpPost]
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
