namespace ZZ.WebAPI.Controllers.Office.Message
{
    public class ReplyRequest : PagingRequestBase
    {
        public string? KeyWord { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? HasRead { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int? ReplyType { get; set; }
    }
}
