namespace ZZ.WebAPI.Controllers.Commons.Department
{
    public record DepartmentRequest(string? DepartmentName, string? Description, int? ParentDepartmenId);
}
