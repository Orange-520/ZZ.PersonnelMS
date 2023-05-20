using ZZ.Domain.Entities.Identity;
using ZZ.Domain.ValueObjects;
using ZZ.DomainCommons.Models;

namespace ZZ.Domain.Entities.Notice
{
	public record Notice : IHasCreationTime , IHasDeletionTime , IHasListModificationTime,ISoftDelete
	{
		public int Id { get; set; }
		public User User { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public NoticeType NoticeType { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set;}
		public DateTime CreationTime { get; init; }
		public DateTime? DeletionTime { get; set; }
		public DateTime? ListModificationTime { get; set; }
		public IsTop IsTop { get; set; } = IsTop.No;

		/// <summary>
		/// 发布范围
		/// </summary>
		public List<Role> Role { get; set; }=new List<Role>();
		public bool IsDeleted { get; set; } = false;

		public Notice() 
		{
			this.CreationTime= DateTime.Now;
		}

		public void SoftDelete()
		{
			this.IsDeleted= true;
			this.DeletionTime = DateTime.Now;
		}
	}
}
