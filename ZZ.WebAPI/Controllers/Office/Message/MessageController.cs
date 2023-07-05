using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
            string userGuid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IQueryable<Reply> sql = db.Replys.Include(e=>e.User).Where(e=>e.User.Id == Guid.Parse(userGuid));

            // 是否模糊搜索
            if (req.KeyWord != null)
            {
                sql = sql.Where(e => e.ReplyContent.Contains(req.KeyWord));
            }

            // 是否限定日期范围
            if (req.StartTime != null && req.EndTime != null)
            {
                if ( req.StartTime > req.EndTime )
                {
					return BadRequest(new ApiResponseBase(400, "开始时间要小于结束时间"));
				}
                sql = sql.Where(e => e.CreationTime >= req.StartTime && e.CreationTime <= req.EndTime );
            }
            else if((req.StartTime != null && req.EndTime == null) || (req.StartTime == null && req.EndTime != null))
            {
                return BadRequest(new ApiResponseBase(400, "如果要选择日期，则必须选择起始日期和结束日期"));
            }

            // 是否限定消息状态
            if (req.HasRead != null)
            {
                HasRead r = ForeachEnum.GetEnumFromInt<HasRead>(req.HasRead);
                if (r == HasRead.None)
                {
                    return BadRequest(new ApiResponseBase(400, "消息状态类型错误"));
                }
                sql = sql.Where(e => e.HasRead == r);
            }

            ReplyType replyType = ForeachEnum.GetEnumFromInt<ReplyType>(req.ReplyType);
            // 不为None则做消息类型限制
            if (replyType != ReplyType.None)
            {
                sql = sql.Where(e=>e.ReplyType == replyType);
            }
            int count = sql.Count();
            var data = sql.Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToList();

            return Ok(new ApiResponseChildB(200,"获取成功", count, data));
        }
    }
}
