using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using ZZ.Domain.Entities.Identity;
using ZZ.Infrastructure;
using ZZ.JWT;

namespace ZZ.WebAPI.Controllers.Identity
{
	[Route("[controller]/[action]")]
	[ApiController]
    public class IdentityController : ControllerBase
	{
		//private readonly IIdentityRepository identityRepository;
		private readonly IdentityRepository identityRepository;

		public IdentityController(IdentityRepository identityRepository)
		{
			this.identityRepository = identityRepository;
		}

		/// <summary>
		/// 初始化管理员
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> CreateWord()
		{
			var findUserResult = await this.identityRepository.FindByUserNameAsync("admin");
			// 判断用户名是否存在
			if (!findUserResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400, "已经初始化过了"));
			}

			User? user = new User("admin");
			// 创建用户并设定密码
			var createUserResult = await this.identityRepository.CreateUserAsync(user, "123456");
			if (!createUserResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400, createUserResult.Item2));
			}

			// 创建角色
			var createRoleResult = await this.identityRepository.CreateRoleAsync("admin");
			if (!createRoleResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400,createRoleResult.Item2));
			}

			// 为用户关联角色
			var updateRoleResult = await this.identityRepository.UpdateRoleByUser(user, "admin");
            if (!updateRoleResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400,updateRoleResult.Item2));
			}

			return Ok(new ApiResponseBase(200, "初始化创建管理员成功"));
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromServices]IJWTService jwtService, [FromServices]IOptions<JWTOptions> jwtOption, LoginRequest login)
		{
			// 查找用户
			var findUserResult = await this.identityRepository.FindByUserNameAsync(login.userName);
			if (!findUserResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400,findUserResult.Item2));
			}
			// 验证密码
			var result = await this.identityRepository.CheckPasswordAsync(findUserResult.Item3!, login.password);

			if (result)
			{
				// 查找用户对应的角色列表
				var roles = await this.identityRepository.GetRoleByUserAsync(findUserResult.Item3!);
				var claims = new List<Claim>()
				{
					new Claim(ClaimTypes.Name, findUserResult.Item3!.UserName),
					new Claim(ClaimTypes.NameIdentifier, findUserResult.Item3!.Id.ToString()),
				};
				foreach (var role in roles)
				{
					claims.Add(new Claim(ClaimTypes.Role, role));
				}

                string token = jwtService.BuildJWT(claims,jwtOption.Value);

                return Ok(new ApiResponseChildA(200, "登录成功",token));
			}
			return BadRequest(new ApiResponseBase(400, "密码错误"));
		}

		/// <summary>
		/// 获取用户信息
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> GetUserMessage()
		{
			// 获取此次请求中，Jwt中携带的用户Id。
			string guid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
			// 查找用户
			var findUserResult = await this.identityRepository.GetByUserIdAsync(guid);
			if (!findUserResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400,findUserResult.Item2));
			}

			IList<string> claim = await this.identityRepository.GetRoleByUserAsync(findUserResult.Item3);

			return Ok(new ApiResponseChildA(200, "获取用户的角色信息成功", new { userName = findUserResult.Item3.UserName, nickName = findUserResult.Item3.NickName, claim }));
		}

		/// <summary>
		/// 创建角色
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		[HttpPost]
        [Authorize(Roles =RoleType.A)]
        public async Task<IActionResult> AddRole(RoleRequest req)
		{
			// 创建不存在的角色
			(bool result,string msg) = await this.identityRepository.CreateRoleAsync(req.RoleName);
			if (!result)
			{
				return BadRequest(new ApiResponseBase(400,msg));
			}
			return Ok(new ApiResponseBase(200,msg));
		}

		/// <summary>
		/// 为用户关联角色
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> ForeignRoleAndUser(ForeignRoleAndUserRequest req) 
		{
			// 判断用户是否存在
			var findUserResult = await this.identityRepository.GetByUserIdAsync(req.Id);
			if (!findUserResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400,findUserResult.Item2));
			}
			// 为用户关联角色
			var updateRoleResult = await this.identityRepository.UpdateRoleByUser(findUserResult.Item3,req.RoleName);
            if (!updateRoleResult.Item1)
			{
				return BadRequest(new ApiResponseBase(400,updateRoleResult.Item2));
			}
			return Ok(new ApiResponseBase(200,updateRoleResult.Item2));
		}

		/// <summary>
		/// 获取所有定义的角色
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public IActionResult GetAllRoles()
		{
			List<Role> roles = this.identityRepository.GetAllRoles();
			var data = roles.Select(e => e.Name );
			return Ok(new ApiResponseChildA(200, "获取所有角色成功",data));
		}

		/// <summary>
		/// 根据用户Id获取用户角色
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> GetRoleByUserId(GetRoleByUserRequest req)
		{
            // 获取用户
            var findUserResult = await this.identityRepository.GetByUserIdAsync(req.UserId);
            if (!findUserResult.Item1)
            {
                return BadRequest(new ApiResponseBase(400, findUserResult.Item2));
            }
            // 获取用户对应的角色
            IList<string> roles = await this.identityRepository.GetRoleByUserAsync(findUserResult.Item3);
			return Ok(new ApiResponseChildA(200,"获取角色信息成功",roles));
		}
	}
}
