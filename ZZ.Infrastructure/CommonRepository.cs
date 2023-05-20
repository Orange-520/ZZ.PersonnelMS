using Microsoft.EntityFrameworkCore;
using ZZ.Domain.Entities.Commons;

namespace ZZ.Infrastructure
{
	public class CommonRepository
	{
		private readonly MyDbContext dbContext;

		public CommonRepository(MyDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		/// <summary>
		/// 查找一个部门，根据部门编号
		/// </summary>
		/// <param name="id"></param>
		/// <returns>返回一个部门实体</returns>
		public ValueTask<Department?> FindByDepartmentIdAsync(int? id)
		{
			if (id == null)
			{
				throw new Exception("部门编号不能为空");
			}
			return  this.dbContext.Departments.FindAsync(id);
		}

		/// <summary>
		/// 查找一个部门下是否存在相同名字的职位
		/// </summary>
		/// <param name="positionName"></param>
		/// <param name="departmentId"></param>
		/// <returns></returns>
		public async Task<(bool,string,Department?)> FindPositionNameByDepartment(string positionName,int departmentId)
		{
			Department? d = await this.FindByDepartmentIdAsync(departmentId);
			if (d == null)
			{
				return (false, "部门不存在", null);
			}
			Position? p = await this.dbContext.Positions.FirstOrDefaultAsync(e => e.Department == d && e.Name == positionName);

			if (p != null) {
				return (false, "该部门已存在此职位",null);
			}
			return (true, "该部门下无该职位", d);
		}

		/// <summary>
		/// 获取所有部门，通过递归逐级获取
		/// </summary>
		/// <param name="parentD"></param>
		/// <returns></returns>
		public List<Department> GetAllDepartment(Department parentD) 
		{
			// 获取部门下的所有子部门
			var chilren = this.dbContext.Departments.AsNoTracking().Where(e => e.ParentDepartmen == parentD).ToList();
			if (chilren.Count == 0)
			{
				return chilren;
			}
			foreach (var child in chilren)
			{
				child.ChildrenDepartment = this.GetAllDepartment(child);
			}
			return chilren;
		}
		//如何返回所有部门，每个部门下带有对应的职位？

		/// <summary>
		/// 查找一个部门下的所有职位
		/// </summary>
		/// <param name="parentD"></param>
		/// <returns></returns>
		public dynamic FindPositionByDepartment(Department parentD)
		{
			return this.dbContext.Positions.AsNoTracking().Where(e => e.Department == parentD).Select(e => new { Id = e.Id, Name=e.Name, Description = e.Description}).ToList();
		}

		/// <summary>
		/// 根据职位 Id 查找职位是否存在
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ValueTask<Position?> FindPositionAsync(int id)
		{
			return this.dbContext.Positions.FindAsync(id);
		}

		public Task UpdateDepartmentAsync(Department department)
		{
			throw new NotImplementedException();
		}

		public Task UpdatePositionAsync(Position position)
		{
			throw new NotImplementedException();
		}
	}
}
