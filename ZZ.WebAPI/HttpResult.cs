namespace ZZ.WebAPI
{
	public class HttpResult
	{
		public int code { get; set; }
		public string msg { get; set; }
		public int total { get; set; }
		public object data { get; set; }
		public HttpResult(int code,string msg,int total, object data = null) 
		{ 
			this.code = code;
			this.msg = msg;
			this.total = total;
			this.data = data;
		}
	}
}
