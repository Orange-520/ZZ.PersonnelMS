namespace ZZ.WebAPI.Controllers.Office.UserApply
{
    public class HiringNeedApplyRequest
    {
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }

        /// <summary>
        /// 申请类型
        /// </summary>
        public int ApplyType { get; set; }
        public string Content { get; set; }
        public int NeedPersonCount { get; set; }
    }
}
