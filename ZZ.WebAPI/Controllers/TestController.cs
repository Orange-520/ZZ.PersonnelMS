using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZZ.Domain.Entities.Identity;

namespace ZZ.WebAPI.Controllers
{
	[Route("[controller]/[action]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly UserManager<User> userManager;
		private readonly RoleManager<Role> roleManager;

		public TestController(RoleManager<Role> roleManager, UserManager<User> userManager)
		{
			this.roleManager = roleManager;
			this.userManager = userManager;
		}

		[HttpPost]
		[Authorize(Roles = "admin,abc")] // 角色是admin或abc的用户可以访问

		// 应用多个属性时，访问用户必须是所有指定角色的成员。
		// 以下示例要求用户必须是 PowerUser 和 ControlPanelUser 角色的成员。
		//[Authorize(Roles = "PowerUser")]
		//[Authorize(Roles = "ControlPanelUser")]
		public IActionResult Test()
		{
			return Ok(new { msg = "Ok" });
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult Test2()
		{
			return Ok(new { msg = "Ok" });
		}

		[HttpPost]
		public async Task<IActionResult> Test3(string roleName)
		{
			if (await this.roleManager.RoleExistsAsync(roleName))
			{
				return Ok("存在此角色");
			}
			return Ok("不存在此角色");
		}

		

		//[HttpPost]
		//public Task<IActionResult> Test4(string roleName)
		//{

		//} 
	}
}
