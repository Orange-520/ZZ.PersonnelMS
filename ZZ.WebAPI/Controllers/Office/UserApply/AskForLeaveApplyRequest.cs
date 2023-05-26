namespace ZZ.WebAPI.Controllers.Office.UserApply
{
    public class AskForLeaveApplyRequest
    {
        public int ApplyType { get; set; }
        /// <summary>
        /// 请假类型
        /// </summary>
        public int AskForLeaveType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public string HowLong { get; set; }
        public string Content { get; set; }
        public string CheckUser { get; set; }
    }
}
