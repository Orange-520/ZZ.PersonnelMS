namespace ZZ.Domain.Entities.Commons
{
	/// <summary>
	/// 部门
	/// </summary>
	public record Department   
	{
		public int Id { get; set; }
		public string Name { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string? Description { get; set; }

		/// <summary>
		/// 指定上级部门
		/// </summary>
		public Department? ParentDepartmen { get; set; }

		/// <summary>
		/// 下级部门
		/// </summary>
		public List<Department> ChildrenDepartment { get; set; } = new List<Department>();
	}
}
