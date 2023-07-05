namespace ZZ.WebAPI.Controllers.JoinUs.Resume
{
	public class ResumeListRequest : PagingRequestBase
	{
		/// <summary>
		/// 姓名查询关键字
		/// </summary>
		public string? NameKey { get; set; }

		/// <summary>
		/// 简历结果类型
		/// </summary>
		public int JoinUsResult { get; set; }
	}
}
