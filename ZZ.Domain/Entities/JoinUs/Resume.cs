using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.Identity;
using ZZ.Domain.Entities.Office;
using ZZ.Domain.ValueObjects;

namespace ZZ.Domain.Entities.JoinUs
{
	/// <summary>
	/// 简历
	/// </summary>
	public record Resume : PersonalInformationBase
	{
		public HiringNeedApply? HiringNeedApply { get; set; }

		/// <summary>
		/// 期望薪资
		/// </summary>
		public decimal? SalaryExpectation { get; set; }

		/// <summary>
		/// 应聘者处在哪个环节
		/// </summary>
		public JoinUsStep JoinUsStep { get; set; } = JoinUsStep.简历筛选;

		/// <summary>
		/// 招聘状态
		/// </summary>
		public JoinUsResult JoinUsResult { get; set; } = JoinUsResult.None;
	}
}
