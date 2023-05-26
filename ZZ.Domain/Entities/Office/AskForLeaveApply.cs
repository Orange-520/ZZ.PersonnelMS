using ZZ.Domain.Entities.Identity;
using ZZ.Domain.ValueObjects;

namespace ZZ.Domain.Entities.Office
{
	/// <summary>
	/// 请假单
	/// </summary>
	public record AskForLeaveApply : ApplyBase
	{
		/// <summary>
		/// 请假时长
		/// </summary>
		public string HowLong { get; set; }
		public AskForLeaveType AskForLeaveType { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public AskForLeaveApply() 
		{
			base.CreationTime= DateTime.Now;
		}

		public AskForLeaveApply(User user,string content,User checkUser,string howLong, AskForLeaveType askForLeaveType,DateTime startTime,DateTime endTime) 
		{
			base.ApplyType = ApplyType.AskForLeave;
			base.CreationTime = DateTime.Now;
			base.User = user;
			base.Content = content;
			base.CheckUser= checkUser;
			this.HowLong = howLong;
			this.AskForLeaveType= askForLeaveType;
			this.StartTime = startTime;
			this.EndTime = endTime;
		}
	}
}
