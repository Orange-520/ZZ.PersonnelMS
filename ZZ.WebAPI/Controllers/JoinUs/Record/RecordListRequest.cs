namespace ZZ.WebAPI.Controllers.JoinUs.Record
{
	public class RecordListRequest : Paging
	{
		public string? NameKey { get; set; }
		public int? DepartmentId { get; set; }	
		public int? PositionId { get; set; }
	}
}
