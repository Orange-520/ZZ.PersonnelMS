using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using ZZ.Domain;
using ZZ.Infrastructure;
using ZZ.WebAPI.Filter;

namespace ZZ.WebAPI.Controllers.Commons.Department
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly MyDbContext db;
        private readonly DepartmentRepository departmentRepository;
        private readonly PositionRepository positionRepository;

        public DepartmentController(MyDbContext db, DepartmentRepository departmentRepository, PositionRepository positionRepository)
        {
            this.db = db;
            this.departmentRepository = departmentRepository;
            this.positionRepository = positionRepository;
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> AddDepartment(DepartmentRequest req)
        {
            if (req.DepartmentName == null && req.DepartmentName == "")
            {
                return BadRequest(new ApiResponseBase(400,"部门名称不能为空"));
            }

            // 添加根部门
            if (req.ParentDepartmenId == null)
            {
                var existRootDepartmentResult = await this.departmentRepository.ExistRootDepartmentNameAsync(req.DepartmentName);
                // 如果根部门已存在
                if (existRootDepartmentResult.Item1)
                {
                    return BadRequest(new ApiResponseBase(400, existRootDepartmentResult.Item2));
                }
                var d = new ZZ.Domain.Entities.Commons.Department { Name = req.DepartmentName, Description = req.Description };
                this.db.Departments.Add(d);
                return Ok(new ApiResponseBase(200, "添加根部门成功"));
            }

            // 寻找此上级部门是否存在
            (bool result,string msg , var parentDepartment) = await this.departmentRepository.FindDepartmentByIdAsync(req.ParentDepartmenId);

            if (!result)
            {
                return BadRequest(new ApiResponseBase(400, msg));
            }
            else
            {
                var d = new ZZ.Domain.Entities.Commons.Department()
                {
                    Name = req.DepartmentName!,
                    Description = req.Description,
                    ParentDepartmen = parentDepartment
                };
                this.db.Departments.Add(d);
                return Ok(new ApiResponseBase(200, "添加子部门成功"));
            }
        }

        /// <summary>
		/// 获取所有部门
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public IActionResult GetAllDepartment()
        {
            IEnumerable<ZZ.Domain.Entities.Commons.Department> dParentList = this.db.Departments.Where(e => e.ParentDepartmen == null).ToList();
            if (!dParentList.Any())
            {
                return StatusCode(200, new { msg = "请设立根部门", data = new ArrayList() });
            }
            foreach (var item in dParentList)
            {
                item.ChildrenDepartment = this.departmentRepository.GetAllChildDepartmentByParentDepartment(item);
            }
            return Ok(new ApiResponseChildA(200, "部门列表", dParentList));
        }

        /// <summary>
        /// 获取一个部门的下一级所有子部门
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetChildDepartment(int departmentId)
        {
            (bool result,string msg, var d) = await this.departmentRepository.FindDepartmentByIdAsync(departmentId);
            if (!result)
            {
                return BadRequest(new ApiResponseBase(400,msg));
            }
            d.ChildrenDepartment = this.db.Departments.Where(e=>e.ParentDepartmen == d).ToList();
            return Ok(new ApiResponseChildA(200,msg,d.ChildrenDepartment));
        }

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> UpdateDepartment(int departmentId, DepartmentRequest req)
        {
            (bool result,string msg,var d) = await this.departmentRepository.FindDepartmentByIdAsync(departmentId);
            if (!result)
            {
                return BadRequest(new ApiResponseBase(400, msg));
            }

            // 是否修改部门名称
            if (req.DepartmentName != null && req.DepartmentName != "")
            {
                d.Name= req.DepartmentName;
            }

            // 是否修改上级部门
            if (req.ParentDepartmenId != null)
            {
                var parentD = await this.departmentRepository.FindDepartmentByIdAsync(req.ParentDepartmenId);
                if (!parentD.Item1)
                {
                    return BadRequest(new ApiResponseBase(400, "上级部门不存在"));
                }
                // 修改实体状态
                d.ParentDepartmen= parentD.Item3;
            }

            // 是否修改部门描述
            if (req.Description != null && req.Description !="")
            {
                d.Description= req.Description;
            }

            return Ok(new ApiResponseBase(200,"修改成功"));
        }

        /// <summary>
        /// 删除一个部门
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [UnitOfWork(typeof(MyDbContext))]
        public async Task<IActionResult> DeleteDepartment([FromServices]JoinUsRepository joinUsRepository, [FromServices]OfficeRepository officeRepository,int departmentId)
        {
            (bool result, string msg, var d) = await this.departmentRepository.FindDepartmentByIdAsync(departmentId);
            if (!result)
            {
                return BadRequest(new ApiResponseBase(400, msg));
            }

            // 删除此部门下的所有子部门
            d.ChildrenDepartment = this.departmentRepository.RemoveAllChildDepartmentByParentDepartment(joinUsRepository, officeRepository, d);

            // 先获取此部门下的职位，将关联这些职位的简历、档案、招聘需求的职位属性设置为null
            var positions = this.positionRepository.GetPositionsByDepartment(d);
            foreach (var position in positions)
            {
                joinUsRepository.UpdateResumeOfPositionSetNull(position);
                joinUsRepository.UpdateRecordOfPositionSetNull(position);
                officeRepository.UpdateHiringNeedApplyOfPositionSetNull(position);
            }

            // 再删除此部门下的所有职位
            this.positionRepository.DeleteAllPositionByDepartment(d);

            // 设置关联此部门的简历实体的求职部门的属性为null
            joinUsRepository.UpdateResumeOfDepartmentSetNull(d);

            // 设置关联此部门的档案实体的所属部门的属性为null
            joinUsRepository.UpdateRecordOfDepartmentSetNull(d);

            // 设置关联此部门的招聘需求实体的部门的属性为null
            officeRepository.UpdateHiringNeedApplyOfDepartmentSetNull(d);

            // 删除此部门
            this.db.Departments.Remove(d);

            return Ok(new ApiResponseBase(200,"删除成功"));
        }
    }
}
