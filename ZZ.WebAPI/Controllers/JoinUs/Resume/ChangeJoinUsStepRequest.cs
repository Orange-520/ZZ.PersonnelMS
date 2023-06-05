namespace ZZ.WebAPI.Controllers.JoinUs.Resume
{
	/// <summary>
	/// 简历环节处理
	/// </summary>
	public class ChangeJoinUsStepRequest
	{
		/// <summary>
		/// 简历Id
		/// </summary>
		public int ResumeId { get; set; }

		/// <summary>
		/// 招聘环节
		/// </summary>
		public int JoinUsStep { get; set; }
	}
}
