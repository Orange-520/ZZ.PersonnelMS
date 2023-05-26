using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZZ.Commons;
using ZZ.Domain.Entities.Office;
using ZZ.Domain.ValueObjects;
using ZZ.Infrastructure;

namespace ZZ.WebAPI.Controllers.Office.Message
{
    [Route("Office/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MyDbContext db;

        public MessageController(MyDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// 获取所有消息提醒
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetAllReply(ReplyRequest req)
        {
            var userGuid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IQueryable<Reply> list = db.Replys.Include(e=>e.User);

            // 是否模糊搜索
            if (req.KeyWord != null)
            {
                list = list.Where(e => e.ReplyContent.Contains(req.KeyWord));
            }

            // 是否限定日期范围
            if (req.StartTime != null && req.EndTime != null)
            {
                if ( req.StartTime > req.EndTime )
                {
					return StatusCode(400, new { code = 400, msg = "开始时间要小于结束时间" });
				}
                list = list.Where(e => e.CreationTime >= req.StartTime && e.CreationTime <= req.EndTime );
            }
            else if((req.StartTime != null && req.EndTime == null) || (req.StartTime == null && req.EndTime != null))
            {
                return StatusCode(400, new { code = 400, msg = "如果要填日期，则必须选择起始日期和结束日期" });
            }

            // 是否限定消息状态
            if (req.HasRead != null)
            {
                HasRead r = ForeachEnum.GetEnumFromInt<HasRead>(req.HasRead);
                if (r == HasRead.None)
                {
                    return StatusCode(400, new { code = 400, msg = "消息状态类型错误" });
                }
                list = list.Where(e => e.HasRead == r);
            }

            ReplyType replyType = ForeachEnum.GetEnumFromInt<ReplyType>(req.ReplyType);
            if (replyType != ReplyType.None)
            {
                list = list.Where(e=>e.ReplyType == replyType);
            }
            int count = list.Count();
            var data = list.Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToList();

            return StatusCode(200, new HttpResult(200,"获取成功", count, data));
        }

    }
}
