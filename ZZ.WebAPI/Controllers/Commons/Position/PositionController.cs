using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.Commons.Position
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PositionController : ControllerBase
    {
        private readonly MyDbContext db;
        private readonly PositionRepository positionRepository;
        private readonly DepartmentRepository departmentRepository;

        public PositionController(MyDbContext db, PositionRepository positionRepository, DepartmentRepository departmentRepository)
        {
            this.db = db;
            this.positionRepository = positionRepository;
            this.departmentRepository = departmentRepository;
        }

        /// <summary>
        /// 添加职位
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> AddPosition(PositionRequest req)
        {
            if (req.DepartmentId == null)
            {
                return BadRequest(new ApiResponseBase(400,"请指定该职位属于哪个部门"));
            }
            if (req.PositionName == null && req.PositionName == "")
            {
                return BadRequest(new ApiResponseBase(400, "请输入职位名称"));
            }

            // 查找所属部门实体
            var d = await this.departmentRepository.FindDepartmentByIdAsync(req.DepartmentId);
            if (!d.Item1)
            {
                return BadRequest(new ApiResponseBase(400, d.Item2));
            }

            // 查找一个部门下是否存在相同名称的职位
            (bool result, string msg) = await this.positionRepository.FindPositionNameByDepartment(req.PositionName!,d.Item3);
            if (!result)
            {
                return StatusCode(400, new ApiResponseBase(400,msg));
            }

            // 创建职位实体
            ZZ.Domain.Entities.Commons.Position p = new()
            {
                Name = req.PositionName!,
                Description = req.Description
            };
            // 指定所属部门
            p.Department = d.Item3!;

            this.db.Positions.Add(p);
            return Ok(new ApiResponseBase(200, "添加职位成功"));
        }

        /// <summary>
		/// 获取某个部门下的职位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetPositionByDepartment([FromQuery(Name = "departmentId")] int departmentId)
        {
            var d = await this.departmentRepository.FindDepartmentByIdAsync(departmentId);
            if (!d.Item1)
            {
                return BadRequest(new ApiResponseBase(400, d.Item2));
            }
            var positions = this.positionRepository.GetPositionsByDepartment(d.Item3);
            var data = positions.Select(e => new { e.Id, e.Name, e.Description });

            return Ok(new ApiResponseChildA(200, "获取成功", data));
        }

        /// <summary>
        /// 修改职位信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> UpdatePosition(int positionId,PositionRequest req)
        {
            // 获取要修改的职位实体
            var p = await this.positionRepository.FindPositionByIdAsync(positionId);
            if (!p.Item1)
            {
                return BadRequest(new ApiResponseBase(400, p.Item2));
            }

            // 是否修改职位名称
            if (req.PositionName != null && req.PositionName != "")
            {
                (bool result,string msg) = await this.positionRepository.FindExistPositionName(positionId, req.PositionName);
                if (result)
                {
                    return BadRequest(new ApiResponseBase(400, msg));
                }
                // 修改实体状态
                p.Item3.Name= req.PositionName;
            }

            // 是否修改职位描述
            if (req.Description != null && req.Description != "")
            {
                p.Item3.Description = req.Description;
            }

            // 是否修改所属部门
            if (req.DepartmentId != null)
            {
                // 查找部门
                (bool result, string msg, var d) = await this.departmentRepository.FindDepartmentByIdAsync(req.DepartmentId);
                if (!result)
                {
                    return BadRequest(new ApiResponseBase(400, msg));
                }
                p.Item3.Department = d;
            }

            return Ok(new ApiResponseBase(200, "修改成功"));
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        [HttpDelete]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> DeletePosition([FromServices]JoinUsRepository joinUsRepository,[FromServices]OfficeRepository officeRepository, int positionId)
        {
            var p = await this.positionRepository.FindPositionByIdAsync(positionId);
            if (!p.Item1)
            {
                return BadRequest(new ApiResponseBase(400, p.Item2));
            }

            joinUsRepository.UpdateResumeOfPositionSetNull(p.Item3);

            joinUsRepository.UpdateRecordOfPositionSetNull(p.Item3);

            officeRepository.UpdateHiringNeedApplyOfPositionSetNull(p.Item3);

            this.db.Positions.Remove(p.Item3);

            return Ok(new ApiResponseBase(200, "删除成功"));
        }
    }
}
