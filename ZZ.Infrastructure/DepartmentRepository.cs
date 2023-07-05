using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZZ.Domain.Entities.Commons;

namespace ZZ.Infrastructure
{
    public class DepartmentRepository
    {
        private readonly MyDbContext db;
        private readonly PositionRepository positionRepository;
        private readonly JoinUsRepository joinUsRepository;

        public DepartmentRepository(MyDbContext db, PositionRepository positionRepository, JoinUsRepository joinUsRepository)
        {
            this.db = db;
            this.positionRepository = positionRepository;
            this.joinUsRepository = joinUsRepository;
        }

        /// <summary>
        /// 判断是否存在相同名称的根部门
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns>存在返回 true</returns>
        public async Task<(bool,string)> ExistRootDepartmentNameAsync(string departmentName)
        {
            var department = await this.db.Departments.FirstOrDefaultAsync(e => e.ParentDepartmen == null && e.Name == departmentName);
            if (department == null)
            {
                return (false,"不存在此根部门");
            }
            return (true,"已存在此根部门");
        }

        /// <summary>
        /// 根据部门Id查找一个部门实体
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns>存在此部门实体则返回的bool值为true</returns>
        public async Task<(bool,string,Department?)> FindDepartmentByIdAsync(int? departmentId)
        {
            if (departmentId == null)
            {
                throw new ArgumentNullException(nameof(departmentId),"部门Id不能为null");
            }
            var d = await this.db.Departments.FindAsync(departmentId);
            if (d == null)
            {
                return (false,"此部门不存在",null);
            }
            return (true,"查找成功",d);
        }

        /// <summary>
		/// 获取一个部门下的所有子部门，直到最小部门，通过递归逐级获取
		/// </summary>
		/// <param name="parentD"></param>
		/// <returns></returns>
		public List<Department> GetAllChildDepartmentByParentDepartment(Department parentD)
        {
            // 获取部门下的所有子部门
            var chilren = this.db.Departments.Where(e => e.ParentDepartmen == parentD).ToList();
            if (!chilren.Any())
            {
                return null;
            }
            foreach (var child in chilren)
            {
                child.ChildrenDepartment = this.GetAllChildDepartmentByParentDepartment(child);
            }
            //思考：如何返回所有部门，每个部门下带有对应的职位？
            return chilren;
        }

        /// <summary>
        /// 删除一个部门下的所有子部门，直到最小部门，通过递归删除
        /// </summary>
        /// <param name="parentD"></param>
        /// <returns></returns>
        public List<Department> RemoveAllChildDepartmentByParentDepartment([FromServices] JoinUsRepository joinUsRepository, [FromServices] OfficeRepository officeRepository,Department parentD)
        {
            // 获取部门下的所有子部门
            var chilren = this.db.Departments.Where(e => e.ParentDepartmen == parentD).ToList();
            if (!chilren.Any())
            {
                return null;
            }
            foreach (var child in chilren)
            {
                child.ChildrenDepartment = this.GetAllChildDepartmentByParentDepartment(child);

                // 先获取此部门下的职位，将关联这些职位的简历、档案、招聘需求的职位属性设置为null
                var positions = this.positionRepository.GetPositionsByDepartment(child);
                foreach (var position in positions)
                {
                    joinUsRepository.UpdateResumeOfPositionSetNull(position);
                    joinUsRepository.UpdateRecordOfPositionSetNull(position);
                    officeRepository.UpdateHiringNeedApplyOfPositionSetNull(position);
                }

                // 再删除此部门下的所有职位
                this.positionRepository.DeleteAllPositionByDepartment(child);

                // 设置关联此部门的实体的部门属性为null
                joinUsRepository.UpdateResumeOfDepartmentSetNull(child);

                joinUsRepository.UpdateRecordOfDepartmentSetNull(child);

                officeRepository.UpdateHiringNeedApplyOfDepartmentSetNull(child);

                // 最后删除此部门
                this.db.Departments.Remove(child);
            }
            return chilren;
        }
    }
}
