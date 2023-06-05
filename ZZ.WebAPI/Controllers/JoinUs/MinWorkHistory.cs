﻿namespace ZZ.WebAPI.Controllers.JoinUs
{
	public class MinWorkHistory
	{
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		/// <summary>
		/// 公司名称
		/// </summary>
		public string CompanyName { get; set; }
		public string CompanyAddress { get; set; }

		/// <summary>
		/// 担任职位
		/// </summary>
		public string Position { get; set; }

		/// <summary>
		/// 离职原因
		/// </summary>
		public string? DimissionCause { get; set; }

		/// <summary>
		/// 最大收获
		/// </summary>
		public string? BiggestGain { get; set; }
	}
}
