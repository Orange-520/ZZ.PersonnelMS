using ZZ.Domain.Entities.Commons;
using ZZ.Domain.ValueObjects;

namespace ZZ.Domain.Entities.JoinUs
{
	/// <summary>
	/// 简历
	/// </summary>
	public record Resume : PersonalInformationBase
	{
		/// <summary>
		/// 应聘部门
		/// </summary>
		public Department Department { get; set; }

		/// <summary>
		/// 应聘职位
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// 期望薪资
		/// </summary>
		public decimal? SalaryExpectation { get; set; }

		/// <summary>
		/// 应聘者处在哪个环节
		/// </summary>
		public JoinUsStep JoinUsStep { get; set; }

		/// <summary>
		/// 招聘状态
		/// </summary>
		public JoinUsResult JoinUsResult { get; set; } = JoinUsResult.None;
	}
}
