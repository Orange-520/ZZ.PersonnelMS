using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using ZZ.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.DomainCommons;
using ZZ.Infrastructure;

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

			var createUserResult = await this.identityRepository.CreateUserAsync(user, "123456");
			if (!createUserResult.Succeeded)
			{
				return StatusCode((int)HttpStatusCode.BadRequest, new { msg = createUserResult.Errors });
			}

			IdentityResult createRoleByUserResult = await this.identityRepository.CreateRoleByUserAsync(user, "Admin");
			if (!createRoleByUserResult.Succeeded)
			{
				return StatusCode((int)HttpStatusCode.BadRequest, new { msg = createRoleByUserResult.Errors });
			}

			return StatusCode((int)HttpStatusCode.OK, new { msg = "初始化创建管理员成功" });
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRequest login)
		{
			// 查找用户
			User? user = await this.identityRepository.FindByUserNameAsync(login.userName);
			if (user == null)
			{
				return StatusCode((int)HttpStatusCode.NotFound, new { msg = "用户不存在" });
			}
			// 验证密码
			var result = await this.identityRepository.CheckPasswordAsync(user, login.password);
			if (result)
			{
				// 如果缓存中不存在，则新建token
				string? token = await this.cache.GetOrCreateAsync(user.UserName + "_token",
					async option =>
					{
						return await this.identityRepository.CreateJWTAsync(user);
					},
					this.jwtSettings.Value.ExpireSeconds);

				return StatusCode((int)HttpStatusCode.OK, new { msg = "登录成功", token });
			}
			else
			{
				return StatusCode((int)HttpStatusCode.BadRequest, new { msg = "密码错误" });
			}
		}

		
	}
}
