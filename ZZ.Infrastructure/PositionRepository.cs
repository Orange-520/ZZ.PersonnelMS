using Microsoft.EntityFrameworkCore;
using ZZ.Domain.Entities.Commons;

namespace ZZ.Infrastructure
{
    public class PositionRepository
    {
        private readonly MyDbContext db;

        public PositionRepository(MyDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// 根据职位id获取一个职位实体
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns>存在职位实体则返回true</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<(bool,string,Position?)> FindPositionByIdAsync(int? positionId)
        {
            if (positionId == null)
            {
                throw new ArgumentNullException(nameof(positionId), "职位Id不能为null");
            }
            var p = await this.db.Positions.FindAsync(positionId);
            if (p == null)
            {
                return (false, "此职位不存在", null);
            }
            return (true, "查找成功", p);
        }

        /// <summary>
        /// 查找一个部门下是否存在与 positionName 相同名称的职位
        /// </summary>
        /// <param name="positionName"></param>
        /// <param name="departmentId"></param>
        /// <returns>不存在相同名称则返回 true</returns>
        public async Task<(bool, string)> FindPositionNameByDepartment(string positionName, Department d)
        {
            // 查找部门下是否存在相同名称的职位
            Position? p = await this.db.Positions.FirstOrDefaultAsync(e => e.Department == d && e.Name == positionName);

            if (p == null)
            {
                return (true, "该部门下无此职位");
            }
            return (false, "该部门已存在此职位");
        }

        /// <summary>
        /// 查找一个部门下是否存在除本身外，还有相同名称的职位
        /// </summary>
        /// <param name="positionId"></param>
        /// <param name="positionName"></param>
        /// <returns>存在返回 true</returns>
        public async Task<(bool, string)> FindExistPositionName(int positionId, string positionName)
        {
            var p = await this.db.Departments.FirstOrDefaultAsync(e => e.Id != positionId && e.Name == positionName);
            if (p == null)
            {
                return (false, "该部门不存在相同的职位名称");
            }
            return (true, "该部门已存在相同的职位名称");
        }

        /// <summary>
        /// 获取一个部门下的所有职位
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public IEnumerable<Position> GetPositionsByDepartment(Department d)
        {
            return this.db.Positions.Where(e => e.Department == d).ToList();
        }

        /// <summary>
        /// 删除一个部门下的所有职位，需要调用 SaveChanges() 保存实体修改后的状态
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public void DeleteAllPositionByDepartment(Department d)
        {
            var positions = this.GetPositionsByDepartment(d);
            foreach (var position in positions)
            {
                this.db.Positions.Remove(position);
            }
        }
    }
}
