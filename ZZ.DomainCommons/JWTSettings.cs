namespace ZZ.DomainCommons
{
	public class JWTSettings
	{
		public string SigningKey { get; set; }
		public int ExpireSeconds { get; set; }
	}
}
