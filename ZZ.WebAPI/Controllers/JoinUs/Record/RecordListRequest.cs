namespace ZZ.WebAPI.Controllers.JoinUs.Record
{
	public class RecordListRequest : PagingRequestBase
	{
		public string? NameKey { get; set; }
		public int? DepartmentId { get; set; }	
		public int? PositionId { get; set; }
	}
}
