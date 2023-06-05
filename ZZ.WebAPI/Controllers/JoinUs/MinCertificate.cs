namespace ZZ.WebAPI.Controllers.JoinUs
{
	/// <summary>
	/// 证书
	/// </summary>
	public class MinCertificate
	{
		public string Name { get; set; }

		/// <summary>
		/// 证书获取时间
		/// </summary>
		public DateTime GetTime { get; set; }

		/// <summary>
		/// 级别
		/// </summary>
		public string Level { get; set; }

		/// <summary>
		/// 证书编号
		/// </summary>
		public string CertificateNumber { get; set; }

		/// <summary>
		/// 颁发机构
		/// </summary>
		public string Organization { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		public string? Remark { get; set; }
	}
}
