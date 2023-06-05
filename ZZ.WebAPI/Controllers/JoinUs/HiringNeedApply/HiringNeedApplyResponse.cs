namespace ZZ.WebAPI.Controllers.JoinUs.HiringNeedApply
{
    public class HiringNeedApplyResponse
    {
        public int Id { get; set; }
        public string applyUserName { get; set; }
        public MinDepartment MinDepartment { get; set; }
        public MinPosition MinPosition { get; set; }
        public string Content { get; set; }
        public int NeedPersonCount { get; set; }
        public int HasPersonCount { get; set; }
        public List<ZZ.Domain.Entities.JoinUs.Resume>? ResumeList { get; set; } = new List<Domain.Entities.JoinUs.Resume>();
    }
    public class MinDepartment
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
    }
    public class MinPosition
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
    }
    
}
