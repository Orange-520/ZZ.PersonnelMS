namespace ZZ.WebAPI.Controllers.Office.Approve
{
    public class CheckApplyRequest
    {
        /// <summary>
        /// 审核状态
        /// </summary>
        public int CheckState { get; set; }

        /// <summary>
        /// 申请的Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 审核申请的类型
        /// </summary>
        public int ApplyType { get; set; }

        /// <summary>
        /// 回复标题
        /// </summary>
        public string ReplyTitle { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }
    }
}
