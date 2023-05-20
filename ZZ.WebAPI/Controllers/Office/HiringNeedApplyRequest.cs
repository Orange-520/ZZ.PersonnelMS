namespace ZZ.WebAPI.Controllers.Office
{
	public class HiringNeedApplyRequest
	{
		public int DepartmentId { get; set; }
		public int PositionId { get; set; }
		public int ApplyType { get; set; }
		public string Content { get; set; }
		public int NeedPersonCount { get; set; }
	}
}
