using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ZZ.Domain.Entities.Identity;
using ZZ.JWT;

namespace ZZ.WebAPI.Controllers
{
	[Route("[controller]/[action]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly UserManager<User> userManager;
		private readonly RoleManager<Role> roleManager;
		private readonly IOptions<JWTOptions> jwtSettings;
        public TestController(UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<JWTOptions> jwtSettings)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.jwtSettings = jwtSettings;
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



		[HttpGet]
		[Authorize]
		public async Task<string> Test4()
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
		public async Task<IActionResult> Test5()
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
				id = id.Value,
				name = userName.Value,
				roles = roles
			});

			//var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId").Value; // 获取userId声明的值

			//string userGuid = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			//User user = await this.identityRepository.GetByUserAsync(userGuid);
		}

		[HttpGet]
		public IActionResult Test6()
		{
			return Ok(new { jwtSettings });
		}

		[HttpGet]
		[Authorize]
		public object Test7()
		{
			var sb = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
           return new JwtSecurityTokenHandler().ReadJwtToken(sb);
        }
	}
}
