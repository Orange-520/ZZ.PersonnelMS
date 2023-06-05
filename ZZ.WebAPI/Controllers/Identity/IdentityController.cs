using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using ZZ.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.JWT;
using ZZ.Infrastructure;
using System.Security.Claims;

namespace ZZ.WebAPI.Controllers.Identity
{
	[Route("[controller]/[action]")]
	[ApiController]
	public class IdentityController : ControllerBase
	{
		//private readonly IIdentityRepository identityRepository;
		private readonly IdentityRepository identityRepository;
		// Distribuited : 分布式的
		private readonly IDistributedCacheHelper cache;
		private readonly IOptions<JWTSettings> jwtSettings;

		public IdentityController(IdentityRepository identityRepository, IDistributedCacheHelper cache, IOptions<JWTSettings> jwtSettings)
		{
			this.identityRepository = identityRepository;
			this.cache = cache;
			this.jwtSettings = jwtSettings;
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> CreateWord()
		{
			User user1 = await this.identityRepository.FindByUserNameAsync("admin");
			// 判断用户名是否存在
			if (user1 != null)
			{
				return StatusCode((int)HttpStatusCode.BadRequest, new { msg = "已经初始化过了" });
			}

			User? user = new User("admin");
			// 创建用户并设定密码
			var createUserResult = await this.identityRepository.CreateUserAsync(user, "123456");
			if (!createUserResult.Succeeded)
			{
				return StatusCode(400, new { msg = createUserResult.Errors });
			}
			// 为用户关联角色
			(bool result,string? msg) = await this.identityRepository.CreateRoleByUserAsync(user, "Admin");
			if (!result)
			{
				return BadRequest(new { code = 400,msg});
			}

			return StatusCode(200, new { msg = "初始化创建管理员成功" });
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Login(LoginRequest login)
		{
			// 查找用户
			User? user = await this.identityRepository.FindByUserNameAsync(login.userName);
			if (user == null)
			{
				return StatusCode(404, new { code=200,msg = "用户不存在" });
			}
			// 验证密码
			var result = await this.identityRepository.CheckPasswordAsync(user, login.password);

			if (result)
			{
				// 如果缓存中不存在，则新建token
				//string? token = await this.cache.GetOrCreateAsync(user.UserName + "_token",
				//	async option =>
				//	{
				//		return await this.identityRepository.CreateJWTAsync(user);
				//	},
				//	this.jwtSettings.Value.ExpireSeconds);

				// 查找用户对应的角色列表
				var roles = await this.identityRepository.GetRoleByUserAsync(user);
				var claims = new List<Claim>()
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				};
				foreach (var role in roles)
				{
					claims.Add(new Claim(ClaimTypes.Role, role));
				}
				// 将Claim信息添加到 Jwt 中，生成Jwt
				string token = await this.identityRepository.CreateJWTAsync(user, claims);

				return Ok(new {code = 200, msg = "登录成功", token });
			}
			return StatusCode(400, new { code = 400, msg = "密码错误" });
		}

		/// <summary>
		/// 创建角色
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles="admin")]
		public async Task<IActionResult> AddRole(RoleRequest req)
		{
			// 创建角色
			(bool result,string msg) = await this.identityRepository.CreateRoleAsync(req.RoleName);
			if (!result)
			{
				return BadRequest(new {code = 400,msg});
			}
			return Ok(new { code = 200, msg});
		}

		/// <summary>
		/// 为用户关联角色
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> ForeignRoleAndUser(ForeignRoleAndUserRequest req) 
		{
			// 判断用户是否存在
			User user = await this.identityRepository.GetByUserIdAsync(req.Id);
			if (user == null)
			{
				return BadRequest(new {code=400,msg="用户不存在"});
			}
			// 为用户关联角色
			(bool result,string msg) = await this.identityRepository.ForeignRoleAndUser(user,req.RoleName);
			if (!result)
			{
				return BadRequest(new { code = 400, msg });
			}
			return Ok(new { code = 200, msg });
		}

		/// <summary>
		/// 获取用户角色信息
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> GetRoleByUser()
		{
			// 获取此次请求中，Jwt中携带的用户Id。
			string guid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			if (guid == null)
			{
				return BadRequest(new { code = 400, msg = "Jwt错误，小子你动Jwt干什么！" });
			}
			// 查找用户
			User user = await this.identityRepository.GetByUserIdAsync(guid);
			if (user == null)
			{
				return BadRequest(new { code = 400,msg ="Jwt错误，小子你动Jwt干什么！"});
			}
			IList<string> claim = await this.identityRepository.GetRoleByUserAsync(user);
			return Ok(new { code= 200,msg = "获取用户的角色信息成功",data = claim });
		}
	}
}
