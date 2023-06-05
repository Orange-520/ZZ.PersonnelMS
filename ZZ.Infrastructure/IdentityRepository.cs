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
		public async Task<string> CreateJWTAsync(User user,List<Claim> claims)
		{
			//// 获取签名密钥
			//string key = jwtSettingsOpt.Value.SigningKey;
			//// 准备过期时间
			////DateTime expires = DateTime.Now.AddSeconds(jwtSettingsOpt.Value.ExpireSeconds);
			//// 将指定字符串中的所有字符编码为字节序列。
			//byte[] secBytes = Encoding.UTF8.GetBytes(key);
			//// 对称安全密钥
			//var seckey = new SymmetricSecurityKey(secBytes);
			//// 定义Microsoft.IdentityModel.Tokens。用于数字签名的密钥、算法和摘要。
			//var credentials = new SigningCredentials(seckey, SecurityAlgorithms.HmacSha256Signature);
			//// 不添加过期时间
			////var tokenDescriptor = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: credentials);
			//var tokenDescriptor = new JwtSecurityToken(claims: claims, signingCredentials: credentials);
			//// JwtSecurityTokenHandler类：用于创建和验证 JWT 。WriteToken：生成 JWT。
			//string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

			//var unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

			// JwtRegisteredClaimNames类标准定义了Jwt中Claim的信息，而我使用的是 .NET 定义的 ClaimTypes 类
			//Claim[] claims = new[]
			//{
			//	//new Claim(JwtRegisteredClaimNames.Sub, "1234567890"),
			//	//new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			//	//new Claim(JwtRegisteredClaimNames.Iat, unixTimestamp.ToString(), ClaimValueTypes.Integer64)
			//	new Claim(JwtRegisteredClaimNames.Role)
			//};
			var now = DateTime.UtcNow;

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsOpt.Value.SigningKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: jwtSettingsOpt.Value.Issuer,
				audience: jwtSettingsOpt.Value.Audience,
				claims: claims,
				// 定义JWT令牌还不能被接受的时间的。也就是说，在NotBefore时间之前，JWT令牌将不被认为是合法的，这通常用于避免网络时钟同步问题或者nonce重放的问题。当JWT令牌处于NotBefore时间之后或刚好在NotBefore时间时生成，JWT令牌才可被接受。
				notBefore: now,
				expires: now.Add(TimeSpan.FromMinutes(30)),
				signingCredentials: creds);

			var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

			return jwtToken;
		}

		/// <summary>
		/// 为用户关联角色，角色不存在则新建角色再关联，（我觉得此方法可添加到 Service 层中，因为它组合了仓储的方法）
		/// </summary>
		/// <param name="roleName"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public async Task<(bool, string)> CreateRoleByUserAsync(User user, string roleName)
		{
			(bool result,string msg) = await this.CreateRoleAsync(roleName);
			if (!result)
			{
				return (result, msg);
			}
			return await this.ForeignRoleAndUser(user, roleName);
		}

		/// <summary>
		/// 创建角色
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		public async Task<(bool,string)> CreateRoleAsync(string roleName)
		{
			// 如果角色不存在
			if (!await this.roleManager.RoleExistsAsync(roleName))
			{
				IdentityResult result = await this.roleManager.CreateAsync(new Role() { Name = roleName });
				if (!result.Succeeded)
				{
					return (false, "创建角色失败");
				}
				return (true, "创建角色成功");
			}
			return (false, "角色已存在");
		}

		/// <summary>
		/// 为用户关联角色
		/// </summary>
		/// <param name="user"></param>
		/// <param name="roleName"></param>
		/// <returns></returns>
		public async Task<(bool,string)> ForeignRoleAndUser(User user, string roleName)
		{
			// 为用户关联角色
			var result = await this.userManager.AddToRoleAsync(user, roleName);
			string msg = "用户关联角色成功";
			if (!result.Succeeded)
			{
				result.Errors.Select(e =>
				{
					msg = "";
					msg += e.Description + ";";
					return true;
				});
				return (false, msg);
			}
			return (true,msg);
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
		public Task<User> GetByUserIdAsync(string id)
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
