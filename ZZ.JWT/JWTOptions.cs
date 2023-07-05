namespace ZZ.JWT
{
	public class JWTOptions
	{
		public string SigningKey { get; set; }
		public int ExpireSeconds { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
	}
}
