using ZZ.Domain.ValueObjects;

namespace ZZ.WebAPI.Controllers.JoinUs
{
	public class ResumeRequest
	{
		public string Name { get; set; }
		public Gender Gender { get; set; }
		public string PhoneNumber { get; set; }

		/// <summary>
		/// 籍贯
		/// </summary>
		public string? NativePlace { get; set; }

		public string? IdCard { get; set; }

		/// <summary>
		/// 婚姻状况
		/// </summary>
		public MaritalStatus? MaritalStatus { get; set; }
		public string? Email { get; set; }
		public DateTime? Birthday { get; set; }
		public PoliticsStatus? PoliticsStatus { get; set; }

		/// <summary>
		/// 毕业学校
		/// </summary>
		public string? SchoolOfGraduation { get; set; }
		public CurrentEducation? CurrentEducation { get; set; }

		/// <summary>
		/// 专业
		/// </summary>
		public string? Major { get; set; }

		/// <summary>
		/// 健康状况
		/// </summary>
		public string? HealthCondition { get; set; }

		/// <summary>
		/// 语言能力
		/// </summary>
		public string? LanguageCompetence { get; set; }
		public JobHuntingMode JobHuntingMode { get; set; }

		/// <summary>
		/// 兴趣特长
		/// </summary>
		public string? InterestsAndTalents { get; set; }

		/// <summary>
		/// 专业技能
		/// </summary>
		public string? ProfessionalSkill { get; set; }

		/// <summary>
		/// 现居住地
		/// </summary>
		public string? CurrentAddress { get; set; }

		/// <summary>
		/// 紧急联系人
		/// </summary>
		public string? EmergencyContact { get; set; }

		/// <summary>
		/// 与其关系
		/// </summary>
		public string? RelationshipWith { get; set; }

		/// <summary>
		/// 紧急联系人号码
		/// </summary>
		public string? EmergencyContactPhoneNumber { get; set; }
		/// <summary>
		/// 应聘部门
		/// </summary>
		public int DepartmentId { get; set; }

		/// <summary>
		/// 应聘职位
		/// </summary>
		public int PositionId { get; set; }

		/// <summary>
		/// 期望薪资
		/// </summary>
		public decimal? SalaryExpectation { get; set; }

		public List<MinWorkHistory> MinWorkHistory { get; set; } = new List<MinWorkHistory>();
		public List<MinEducationHistory> MinEducationHistory { get; set; } = new List<MinEducationHistory>();
		public List<MinCertificate> MinCertificate { get; set; } = new List<MinCertificate> { };
	}
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
	public class MinEducationHistory
	{
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

	/// <summary>
	/// 证书
	/// </summary>
	public class MinCertificate
	{
		public string Name { get; set; }

		/// <summary>
		/// 证书获取时间
		/// </summary>
		public DateTime GetTime { get; set; }

		/// <summary>
		/// 级别
		/// </summary>
		public string Level { get; set; }

		/// <summary>
		/// 证书编号
		/// </summary>
		public string CertificateNumber { get; set; }

		/// <summary>
		/// 颁发机构
		/// </summary>
		public string Organization { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		public string? Remark { get; set; }
	}
}
