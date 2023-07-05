using ZZ.Domain.Entities.Identity;
using ZZ.Domain.ValueObjects;
using ZZ.DomainCommons.Models;

namespace ZZ.Domain.Entities.Office
{
	/// <summary>
	/// 回复表
	/// </summary>
	public class Reply : IHasCreationTime,IHasDeletionTime,ISoftDelete
	{
		public int Id { get; set; }

		/// <summary>
		/// 谁的？
		/// </summary>
		public User User { get; set; }

		/// <summary>
		/// 回复人
		/// </summary>
		public User PublisherUser { get; set; }

		/// <summary>
		/// 消息类型
		/// </summary>
		public ReplyType ReplyType { get; set; }

		/// <summary>
		/// 回复标题
		/// </summary>
		public string ReplyTitle { get; set; }

		/// <summary>
		/// 回复内容
		/// </summary>
		public string ReplyContent { get; set; }

		/// <summary>
		/// 是否已读
		/// </summary>
		public HasRead HasRead { get; set; } = HasRead.NoRead;

		public DateTime CreationTime {get; init; }

		public DateTime? DeletionTime {get;private set; }

		public bool IsDeleted {get;private set;}

		public Reply() { }

		public Reply(User user,User publisherUser, ReplyType replyType,string replyTitle,string replyContent)
		{
			this.CreationTime = DateTime.Now;
			this.User = user;
			this.PublisherUser = publisherUser;
			this.ReplyType = replyType;
			this.ReplyTitle= replyTitle;
			this.ReplyContent = replyContent;
		}

		public void SoftDelete()
		{
			this.DeletionTime= DateTime.Now;
			this.IsDeleted= true;
		}

		/// <summary>
		/// 修改未读状态为已读
		/// </summary>
		public void AlterReadState()
		{
			this.HasRead = HasRead.YesRead;
		}
	}
}
