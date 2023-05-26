namespace ZZ.Domain.ValueObjects
{
	/// <summary>
	/// 申请类型
	/// </summary>
	public enum ApplyType
	{
		None,
		/// <summary>
		/// 招聘需求
		/// </summary>
		HiringNeeds,

		/// <summary>
		/// 出差
		/// </summary>
		GoOut,

		/// <summary>
		/// 请假
		/// </summary>
		AskForLeave,

		/// <summary>
		/// 离职
		/// </summary>
		Dimission,

		/// <summary>
		/// 报销
		/// </summary>
		Reimbursement
	}

	/// <summary>
	/// 请假类型
	/// </summary>
	public enum AskForLeaveType
	{
		None,
		事假,
		病假
	}

	/// <summary>
	/// 审核状态
	/// </summary>
	public enum CheckState
	{
		None,
		/// <summary>
		/// 同意
		/// </summary>
		Yes,

		/// <summary>
		/// 不同意
		/// </summary>
		No,

		/// <summary>
		/// 待审核
		/// </summary>
		Wait
	}

	/// <summary>
	/// 消息提醒类型
	/// </summary>
	public enum ReplyType
	{
		None,
		个人申请,
		公告通知,
		人事档案
	}

	/// <summary>
	/// 是否已读
	/// </summary>
	public enum HasRead
	{
		None,
		/// <summary>
		/// 已读
		/// </summary>
		YesRead,
		NoRead
	}

	/// <summary>
	/// 公告类型
	/// </summary>
	public enum NoticeType
	{
		None,
		HolidayNotice,
		OtherNotice
	}

	/// <summary>
	/// 是否置顶
	/// </summary>
	public enum IsTop
	{
		None,
		No, 
		Yes
	}

	/// <summary>
	/// 性别
	/// </summary>
	public enum Gender
	{
		None,
		Man,
		Woman
	}

	/// <summary>
	/// 婚姻状况
	/// </summary>
	public enum MaritalStatus 
	{
		/// <summary>
		/// null
		/// </summary>
		None,

		/// <summary>
		/// 已婚
		/// </summary>
		Married,

		/// <summary>
		/// 未婚
		/// </summary>
		Spinsterhood
	}

	public enum PoliticsStatus
	{
		/// <summary>
		/// null
		/// </summary>
		None,

		中共党员,
		中共预备党员,
		共青团员,
		民革党员,
		民盟盟员,
		民建会员,
		民进会员,
		农工党党员,
		致公党党员,
		九三学社社员,
		台盟盟员,
		无党派人士,
		群众
	}

	public enum JobHuntingMode
	{
		None,
		网上招聘,
		校园招聘,
		专场招聘,
		亲戚朋友推荐
	}

	public enum CurrentEducation
	{
		None,
		小学,
		初中,
		高中,
		大专,
		大学本科,
		研究生
	}

	/// <summary>
	/// 学位
	/// </summary>
	public enum Degree
	{
		/// <summary>
		/// null
		/// </summary>
		None,

		学士,
		硕士,
		博士
	}

	/// <summary>
	/// 用工性质
	/// </summary>
	public enum NatureOfEmployment
	{
		None,
		正式员工,
		临时工,
		实习生
	}

	/// <summary>
	/// 学习方式
	/// </summary>
	public enum LearningStyle
	{
		/// <summary>
		/// null
		/// </summary>
		None,

		全日制,
		在职,
		自考
	}

	/// <summary>
	/// 招聘环节
	/// </summary>
	public enum JoinUsStep
	{
		None,
		简历筛选,
		初试,
		面试,
		背景调查,
		终面,
		录用决策中,
		发出录用通知书,
		入职培训, 
		跟踪反馈
	}

	/// <summary>
	/// 招聘结果
	/// </summary>
	public enum JoinUsResult
	{
		/// <summary>
		/// null
		/// </summary>
		None,

		入职,
		放入人才库,
		放入资料库,
		不合格
	}
}
