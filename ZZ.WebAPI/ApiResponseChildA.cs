namespace ZZ.WebAPI
{
    public class ApiResponseChildA : ApiResponseBase
    {
        public object data { get; set; }
        public ApiResponseChildA(int code, string msg, object data = null) : base(code, msg)
        {
            this.data = data;
        }
    }
}
