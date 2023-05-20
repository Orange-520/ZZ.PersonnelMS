using Microsoft.AspNetCore.Identity;
using ZZ.DomainCommons.Models;

namespace ZZ.Domain.Entities.Identity
{
	/// <summary>
	/// 用户
	/// </summary>
	public class User : IdentityUser<Guid>, IHasCreationTime, IHasDeletionTime, IHasListModificationTime, ISoftDelete
	{
		/// <summary>
		/// 昵称
		/// </summary>
		public string? NickName { get; set; }
		public DateTime CreationTime { get; init; }
		public DateTime? DeletionTime { get; private set; }
		public DateTime? ListModificationTime { get; set; }
		public bool IsDeleted { get; private set; }

		public User(string userName):base(userName)
		{
			base.Id= Guid.NewGuid();
			this.CreationTime = DateTime.Now;
		}
		void ISoftDelete.SoftDelete()
		{
			this.IsDeleted = true;
			this.DeletionTime = DateTime.Now;
		}
	}
}
