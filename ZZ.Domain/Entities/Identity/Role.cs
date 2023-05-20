using Microsoft.AspNetCore.Identity;

namespace ZZ.Domain.Entities.Identity
{
	/// <summary>
	/// 角色
	/// </summary>
	public class Role : IdentityRole<Guid>
	{
		public Role()
		{
			base.Id = Guid.NewGuid();
		}
	}
}
