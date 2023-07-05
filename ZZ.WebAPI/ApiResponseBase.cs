namespace ZZ.WebAPI
{
	public class ApiResponseBase
    {
		public int code { get; set; }
		public string msg { get; set; }

		public ApiResponseBase(int code,string msg)
		{
			this.code = code;
			this.msg = msg;
		}
	}
}
