namespace ZZ.Domain.Entities.Commons
{
	/// <summary>
	/// 职位
	/// </summary>
	public record Position
	{
		public int Id { get; set; }

		/// <summary>
		/// 关联部门类
		/// </summary>
		public Department Department { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
	}
}
