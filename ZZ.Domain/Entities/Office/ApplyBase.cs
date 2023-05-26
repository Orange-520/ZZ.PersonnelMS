using ZZ.Domain.Entities.Identity;
using ZZ.Domain.ValueObjects;
using ZZ.DomainCommons.Models;

namespace ZZ.Domain.Entities.Office
{
	/// <summary>
	/// 基类，用于定义公共的个人申请
	/// </summary>
	public record ApplyBase : IHasCreationTime,IHasDeletionTime,IHasListModificationTime,ISoftDelete
	{
		public int Id { get; set; }

		/// <summary>
		/// 申请人
		/// </summary>
		public User User { get; set; }
		public ApplyType ApplyType { get; set; }
		public string Content { get; set; }

		/// <summary>
		/// 审核状态
		/// </summary>
		public CheckState CheckState { get; set; } = CheckState.Wait;

		/// <summary>
		/// 审核人
		/// </summary>
		public User? CheckUser { get; set; }
		public DateTime CreationTime { get; init; }
		public DateTime? DeletionTime { get;private set; }
		public DateTime? ListModificationTime { get; set; }
		public bool IsDeleted { get; private set; }

		public void SoftDelete()
		{
			this.IsDeleted= true;
			this.DeletionTime= DateTime.Now;
		}

		/// <summary>
		/// 改变实体的审核状态
		/// </summary>
		/// <param name="checkState"></param>
		public void AlterCheckState(CheckState checkState)
		{
			this.CheckState = checkState;
		}
	}
}
