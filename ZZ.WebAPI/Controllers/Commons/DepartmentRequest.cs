namespace ZZ.WebAPI.Controllers.Commons
{
	public record DepartmentRequest(string DepartmentName, string Description,int? ParentDepartmenId);
}
