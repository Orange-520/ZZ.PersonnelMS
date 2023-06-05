using ZZ.Domain.Entities.Identity;
using ZZ.Domain.ValueObjects;
using ZZ.DomainCommons.Models;

namespace ZZ.Domain.Entities.JoinUs
{
	/// <summary>
	/// 档案
	/// </summary>
	public record Record : PersonalInformationBase, ISoftDelete
	{
		/// <summary>
		/// 关联用户
		/// </summary>
		public User User { get; set; }

		/// <summary>
		/// 入职时间
		/// </summary>
		public DateTime EntryTime { get; set; }

		/// <summary>
		/// 相片
		/// </summary>
		public string? Avatar { get; set; }

		public NatureOfEmployment NatureOfEmployment { get; set; }

		/// <summary>
		/// 身份证复印件图片
		/// </summary>
		public string? IdCardPicture { get; set; }

		/// <summary>
		/// 转正日期
		/// </summary>
		public DateTime? DateOfConfirmationAfterProbation { get; set; }

		/// <summary>
		/// 购买社保类型
		/// </summary>
		public string? TypeOfSocialSecurity { get; set; }
		
		/// <summary>
		/// 社保卡号
		/// </summary>
		public string? SocialSecurityCardNumber { get; set; }

		public bool IsDeleted { get; private set; } = false;

		/// <summary>
		/// 离职日期
		/// </summary>
		public DateTime? LeaveTime { get; set; }

		/// <summary>
		/// 两个文件的大小和散列值（SHA256）都相同的概率非常小。因此只要大小和SHA256相同，就认为是相同的文件。
		/// SHA256的碰撞的概率比MD5低很多。
		/// </summary>
		public string? AvatarFileSHA256Hash { get; set; }

		public void SoftDelete()
		{
			this.IsDeleted= true;
		}
	}
}
