namespace ZZ.WebAPI.Controllers.JoinUs.Resume
{
	/// <summary>
	/// 简历处理结果
	/// </summary>
	public class ChangeJoinUsResultRequest
	{
		/// <summary>
		/// 简历Id
		/// </summary>
		public int ResumeId { get; set; }

		/// <summary>
		/// 结果
		/// </summary>
		public int JoinUsResult { get; set; }
	}
}
