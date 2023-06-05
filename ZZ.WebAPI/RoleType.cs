namespace ZZ.WebAPI
{
	public static class RoleType
	{
		// const 关键字有静态之意

		/// <summary>
		/// 管理员
		/// </summary>
		public const string A = "admin";

		/// <summary>
		/// 人事部主管
		/// </summary>
		public const string B = "personnelInCharge";

		/// <summary>
		/// 其它部门主管
		/// </summary>
		public const string C = "itInCharge";

		/// <summary>
		/// 人事部员工
		/// </summary>
		public const string D = "personnel";

		/// <summary>
		/// 其它部门员工
		/// </summary>
		public const string E = "it";
	}
}
