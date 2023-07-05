using ZZ.Domain.ValueObjects;

namespace ZZ.WebAPI.Controllers.JoinUs.Resume
{
    public class AddResumeRequest
    {
        public int? HiringNeedApplyId { get; set; }
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
}
