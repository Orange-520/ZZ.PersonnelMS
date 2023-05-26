using ZZ.Domain.Entities.Identity;
using ZZ.Domain.ValueObjects;

namespace ZZ.WebAPI.Controllers.Office.Approve
{
    /// <summary>
    /// 申请响应
    /// </summary>
    public class ApplyResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public User User { get; set; }
        public DateTime CreateTime { get; set; }
        public ApplyType ApplyType { get; set; }
        public string Content { get; set; }

    }
}
