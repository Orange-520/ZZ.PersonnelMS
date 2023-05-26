namespace ZZ.WebAPI.Controllers.Office.Approve
{
    /// <summary>
    /// 申请详情
    /// </summary>
    public class ApplyDetailRequest
    {
        /// <summary>
        /// 申请id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 申请类型
        /// </summary>
        public int ApplyType { get; set; }
    }
}
