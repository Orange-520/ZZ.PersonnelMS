using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZZ.Domain.Entities.Identity;
using ZZ.JWT;

namespace ZZ.Infrastructure
{
	public class IdentityRepository
	{
		private readonly UserManager<User> userManager;
		private readonly RoleManager<Role> roleManager;
		private readonly MyDbContext dbContext;
		private readonly IOptions<JWTSettings> jwtSettingsOpt;
		private readonly ILogger<IdentityRepository> logger;

		public IdentityRepository(MyDbContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<JWTSettings> jwtSettingsOpt, ILogger<IdentityRepository> logger)
		{
			this.dbContext = dbContext;
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.jwtSettingsOpt = jwtSettingsOpt;
			this.logger = logger;
		}

		private static IdentityResult ErrorResult(string msg)
		{
			IdentityError error = new IdentityError { Description = msg };
			return IdentityResult.Failed(error);
		}

		/// <summary>
		/// 生成JWT
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public async Task<string> CreateJWTAsync(User user)
		{
			var roles = await GetRoleByUserAsync(user);
			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.Name, user.UserName));
			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			// 获取签名密钥
			string key = jwtSettingsOpt.Value.SigningKey;
			// 准备过期时间
			//DateTime expires = DateTime.Now.AddSeconds(jwtSettingsOpt.Value.ExpireSeconds);
			// 将指定字符串中的所有字符编码为字节序列。
			byte[] secBytes = Encoding.UTF8.GetBytes(key);
			// 对称安全密钥
			var seckey = new SymmetricSecurityKey(secBytes);
			// 定义Microsoft.IdentityModel.Tokens。用于数字签名的密钥、算法和摘要。
			var credentials = new SigningCredentials(seckey, SecurityAlgorithms.HmacSha256Signature);
			// 不添加过期时间
			//var tokenDescriptor = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: credentials);
			var tokenDescriptor = new JwtSecurityToken(claims: claims, signingCredentials: credentials);
			// JwtSecurityTokenHandler类：用于创建和验证 JWT 。WriteToken：生成 JWT。
			string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

			return jwt;
		}

		/// <summary>
		/// 为用户关联角色，角色不存在则新建角色再关联
		/// </summary>
		/// <param name="roleName"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public async Task<IdentityResult> CreateRoleByUserAsync(User user, string roleName)
		{
			// 如果角色不存在
			if (!await this.roleManager.RoleExistsAsync(roleName))
			{
				IdentityResult result = await this.roleManager.CreateAsync(new Role() { Name = roleName });
				if (result.Succeeded == false)
				{
					return result;
				}
			}
			return await this.userManager.AddToRoleAsync(user, roleName);
		}
		/// <summary>
		/// 创建角色
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		public async Task<IdentityResult> CreateRoleAsync(string roleName)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 创建用户并设定密码
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public Task<IdentityResult> CreateUserAsync(User user, string password)
		{
			return this.userManager.CreateAsync(user,password);
		}

		/// <summary>
		/// 根据用户名查找用户
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		public Task<User> FindByUserNameAsync(string userName)
		{
			return this.userManager.FindByNameAsync(userName);
			
		}

		/// <summary>
		/// 根据用户Id查找用户
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Task<User> GetByUserAsync(string id)
		{
			return this.userManager.FindByIdAsync(id);
		}

		/// <summary>
		/// 查找用户对应的角色
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public Task<IList<string>> GetRoleByUserAsync(User user)
		{
			return this.userManager.GetRolesAsync(user);
		}

		/// <summary>
		/// 验证用户密码
		/// </summary>
		/// <param name="user"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public Task<bool> CheckPasswordAsync(User user,string password)
		{
			return this.userManager.CheckPasswordAsync(user,password);
		}
	}
}
