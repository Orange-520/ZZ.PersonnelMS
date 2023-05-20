using ZZ.Domain.Entities.Commons;

namespace ZZ.Domain
{
	public interface ICommonRepository
	{
		Task CreateDepartmentAsync(Department department);
		Task CreatePositionAsync(Position position);
		Task UpdateDepartmentAsync(Department department);
		Task UpdatePositionAsync(Position position);
	}
}
