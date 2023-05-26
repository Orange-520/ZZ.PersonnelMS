using ZZ.Domain.Entities.Identity;
using ZZ.Domain.ValueObjects;

namespace ZZ.WebAPI.Controllers.Office.UserApply
{
    /// <summary>
    /// 用户申请列表响应类
    /// </summary>
    public class UserApplyResponse
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        public int Id { get; set; }
        public ApplyType ApplyType { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public CheckState CheckState { get; set; }
        public User? CheckUser { get; set; }
    }
}
