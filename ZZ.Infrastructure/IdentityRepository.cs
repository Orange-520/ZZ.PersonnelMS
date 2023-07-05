using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ZZ.Domain.Entities.Identity;

namespace ZZ.Infrastructure
{
	public class IdentityRepository
	{
		private readonly UserManager<User> userManager;
		private readonly RoleManager<Role> roleManager;
		private readonly ILogger<IdentityRepository> logger;

		public IdentityRepository( UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<IdentityRepository> logger)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.logger = logger;
		}

		/// <summary>
		/// 创建不存在的角色
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		public async Task<(bool,string)> CreateRoleAsync(string roleName)
		{
			// 如果角色不存在
			if (!await this.roleManager.RoleExistsAsync(roleName))
			{
				IdentityResult result = await this.roleManager.CreateAsync(new Role() { Name = roleName });
				if (result.Succeeded)
				{
                    return (true, "创建角色成功");
				}
                return (false, "创建角色失败");
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
		/// 删除用户原角色信息，更新用户绑定的角色
		/// </summary>
		/// <param name="user"></param>
		/// <param name="roleName"></param>
		/// <returns></returns>
		public async Task<(bool,string)> UpdateRoleByUser(User user,string roleName)
		{
			// 判断角色是否存在
			if (!await this.roleManager.RoleExistsAsync(roleName))
			{
				return (false,"角色不存在");
			}

			IList<string> roles = await this.GetRoleByUserAsync(user);
			// 判断用户是否已绑定角色
			if (roles.Count() != 0)
			{
                // 先删除用户原绑定的角色
                await this.userManager.RemoveFromRolesAsync(user, roles);
            }

            // 再为用户绑定角色
			return await this.ForeignRoleAndUser(user, roleName);
        }

		/// <summary>
		/// 创建用户并设定密码
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public async Task<(bool,string)> CreateUserAsync(User user, string password)
		{
			var result = await this.userManager.CreateAsync(user, password);
			if (result.Succeeded)
			{
				return (true, "创建用户成功");
			}
			string msg = result.Errors.Select(e => e.Description+";").ToString();
			return (false,msg);
		}

		/// <summary>
		/// 根据用户名查找用户
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		public async Task<(bool,string,User?)> FindByUserNameAsync(string userName)
		{
            User u = await this.userManager.FindByNameAsync(userName);
			if (u == null)
			{
				return (false, "用户不存在", null);
			}
            return (true, "查找成功", u);
        }

		/// <summary>
		/// 根据用户Id查找用户
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<(bool, string, User?)> GetByUserIdAsync(string userId)
		{
            User u = await this.userManager.FindByIdAsync(userId);
            if (u == null)
            {
                return (false, "用户不存在", null);
            }
            return (true, "查找成功", u);
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
		
		/// <summary>
		/// 获取所有角色
		/// </summary>
		/// <returns></returns>
		public List<Role> GetAllRoles()
		{
			return this.roleManager.Roles.ToList();
		}
	}
}
