namespace ZZ.WebAPI
{
	public class ApiResponseChildB : ApiResponseBase
	{
		public int total { get; set; }
		public object data { get; set; }
		public ApiResponseChildB(int code,string msg,int total, object data = null):base(code,msg)
		{ 
			this.total = total;
			this.data = data;
		}
	}
}
