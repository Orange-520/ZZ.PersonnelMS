using System.ComponentModel.DataAnnotations;

namespace ZZ.WebAPI.Controllers.JoinUs
{
	public record AddRecordRequest
    {
		/// <summary>
		/// 账号
		/// </summary>
		[Required]
        public string UserName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[Required]
		public string Password { get; set; }

		/// <summary>
		/// 应聘部门
		/// </summary>
		[Required]
		public int DepartmentId { get; set; }

		/// <summary>
		/// 应聘职位
		/// </summary>
		[Required]
		public int PositionId { get; set; }

		/// <summary>
		/// 入职时间
		/// </summary>
		[Required]
		public DateTime EntryTime { get; set; }

		/// <summary>
		/// 相片
		/// </summary>
		public string? Avatar { get; set; }

		/// <summary>
		/// 用工性质
		/// </summary>
		[Required]
		public int NatureOfEmployment { get; set; }

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

		public List<MinWorkHistory> MinWorkHistory { get; set; } = new List<MinWorkHistory>();
		public List<MinEducationHistory> MinEducationHistory { get; set; } = new List<MinEducationHistory>();
		public List<MinCertificate> MinCertificate { get; set; } = new List<MinCertificate> { };

		public string Name { get; set; }
		public int Gender { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// 籍贯
		/// </summary>
		public string? NativePlace { get; set; }

		[Required]
		public string IdCard { get; set; }

		/// <summary>
		/// 婚姻状况
		/// </summary>
		[Required]
		public int MaritalStatus { get; set; }

		[Required]
		public string Email { get; set; }

		public DateTime? Birthday { get; set; }

		/// <summary>
		/// 政治面貌
		/// </summary>
		public int? PoliticsStatus { get; set; }

		/// <summary>
		/// 毕业学校
		/// </summary>
		public string? SchoolOfGraduation { get; set; }

		/// <summary>
		/// 当前学历
		/// </summary>
		[Required]
		public int CurrentEducation { get; set; }

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

		/// <summary>
		/// 求职方式
		/// </summary>
		[Required]
		public int JobHuntingMode { get; set; }

		/// <summary>
		/// 兴趣特长
		/// </summary>
		public string? InterestsAndTalents { get; set; }

		/// <summary>
		/// 专业技能
		/// </summary>
		[Required]
		public string ProfessionalSkill { get; set; }

		/// <summary>
		/// 现居住地
		/// </summary>
		[Required]
		public string CurrentAddress { get; set; }

		/// <summary>
		/// 紧急联系人
		/// </summary>
		[Required]
		public string EmergencyContact { get; set; }

		/// <summary>
		/// 与其关系
		/// </summary>
		[Required]
		public string RelationshipWith { get; set; }

		/// <summary>
		/// 紧急联系人号码
		/// </summary>
		[Required]
		public string EmergencyContactPhoneNumber { get; set; }
	}
}
