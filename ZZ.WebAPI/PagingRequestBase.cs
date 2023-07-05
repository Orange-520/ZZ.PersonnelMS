namespace ZZ.WebAPI
{
	public class PagingRequestBase
	{
		/// <summary>
		/// 第几页开始
		/// </summary>
		public int PageIndex { get; set; }

		/// <summary>
		/// 每页的记录数
		/// </summary>
		public int PageSize { get; set; }
	}
}
