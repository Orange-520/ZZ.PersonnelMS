namespace ZZ.Domain.Entities.Office
{
	/// <summary>
	/// 请假单
	/// </summary>
	public record AskForLeaveApply : ApplyBase
	{
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public AskForLeaveApply() 
		{
			base.CreationTime= DateTime.Now;
		}
	}
}
