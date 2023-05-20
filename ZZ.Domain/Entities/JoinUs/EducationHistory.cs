﻿using ZZ.Domain.Entities.Identity;
using ZZ.Domain.ValueObjects;

namespace ZZ.Domain.Entities.JoinUs
{
	/// <summary>
	/// 教育经历
	/// </summary>
	public record EducationHistory
	{
		public int Id { get; set; }
		public string SchoolName { get; set; }
		public CurrentEducation CurrentEducation { get; set; }

		/// <summary>
		/// 专业
		/// </summary>
		public string? Major { get; set; }

		/// <summary>
		/// 入学时间
		/// </summary>
		public DateTime StartTime { get; set; }

		/// <summary>
		/// 毕业时间
		/// </summary>
		public DateTime EndTime { get; set; }

		/// <summary>
		/// 学位
		/// </summary>
		public Degree? Degree { get; set; }

		/// <summary>
		/// 学位授予时间
		/// </summary>
		public DateTime? DegreeCreateTime { get; set; }
		public LearningStyle LearningStyle { get; set; }
		public float Score { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		public string? Remark { get; set; }
	}
}