using Microsoft.AspNetCore.Identity;
using ZZ.Domain.Entities.Identity;

namespace ZZ.Domain
{
	/// <summary>
	/// 身份认证服务仓储接口
	/// </summary>
	public interface IIdentityRepository
	{
		Task<string> CreateJWTAsync(User user);
		Task<IdentityResult> CreateRoleByUserAsync(User user, string roleName);
		Task<IdentityResult> CreateRoleAsync(string roleName);
		Task<IdentityResult> CreateUserAsync(User user, string password);
		Task<User> FindByUserAsync(string userName);
		Task<User> GetByUserAsync(Guid id);
		Task<IList<string>> GetRoleByUserAsync(User user);
		Task<bool> CheckPasswordAsync(User user, string password);
	}
}
