using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.JoinUs;
using ZZ.Domain.ValueObjects;

namespace ZZ.Domain.Entities.Office
{
	/// <summary>
	/// 招聘需求
	/// </summary>
	public record HiringNeedApply : ApplyBase
	{
		/// <summary>
		/// 应聘部门
		/// </summary>
		public Department Department { get; set; }

		/// <summary>
		/// 岗位
		/// </summary>
		public Position Position { get; set; }
		public int NeedPersonCount { get; set; }
		public int HasPersonCount { get; set; } = 0;
		public List<Resume>? CurrentResumes { get; set; } = new List<Resume>();

		/// <summary>
		/// 如果实体类中声明了其它的构造函数，需要准备一个无参构造函数供 EF Core 使用
		/// </summary>
		public HiringNeedApply() { }
		public HiringNeedApply(User user,ApplyType applyType,string content,User? checkUser,Department department,Position position,int needPersonCount)
		{
			base.User = user;
			base.ApplyType = applyType;
			base.Content = content;
			base.CheckUser = checkUser;
			base.CreationTime = DateTime.Now;
			this.Department = department;
			this.Position = position;
			this.NeedPersonCount = needPersonCount;
			
		}
	}
}
