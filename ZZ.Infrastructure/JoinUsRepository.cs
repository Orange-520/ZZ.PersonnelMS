using Microsoft.EntityFrameworkCore;
using ZZ.Domain.Entities.Commons;
using ZZ.Domain.Entities.JoinUs;

namespace ZZ.Infrastructure
{
    public class JoinUsRepository
	{
		private readonly MyDbContext db;

		public JoinUsRepository(MyDbContext db)
		{
			this.db = db;
		}

        /// <summary>
        /// 批量修改档案实体的所属部门属性为null，运行报错，原因：无法翻译
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public async Task UpdateRecordOfDepartmentSetNullAsync(Department d)
		{
            // ExecuteUpdateAsync方法的SetProperty方法写法二：无法通过编译
            //await this.db.Records.Where(e => e.Department == d).ExecuteUpdateAsync(setter => setter.SetProperty(b => b.Department,null));

            //FormattableString sql = $@"update T_Records set DepartmentId = null where DepartmentId = {d.Id}";
            //await this.db.Database.ExecuteSqlInterpolatedAsync(sql);
        }

        /// <summary>
		/// 批量修改档案实体的所属职位属性为null，运行报错，原因：无法翻译
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public async Task UpdateRecordOfPositionSetNullAsync(Position p)
        {
            await this.db.Records.Where(e => e.Position == p).ExecuteUpdateAsync(s => s.SetProperty(b => b.Position, b => null));
        }

        /// <summary>
        /// 批量修改简历实体的所属部门属性为null，运行报错，原因：无法翻译
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public async Task UpdateResumeOfDepartmentSetNullAsync(Department d)
        {
           await this.db.Resumes.Where(e => e.Department == d).ExecuteUpdateAsync(s =>s.SetProperty(b => b.Department, b => null));
        }

        /// <summary>
		/// 批量修改简历实体的所属职位属性为null，运行报错，原因：无法翻译
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public async Task UpdateResumeOfPositionSetNullAsync(Position p)
        {
            await this.db.Resumes.Where(e => e.Position == p).ExecuteUpdateAsync(s => s.SetProperty(b => b.Position, b => null));
        }

        /// <summary>
        /// 批量修改档案实体的所属部门属性为null，执行多次sql版本
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public void UpdateRecordOfDepartmentSetNull(Department d)
        {
            IQueryable<Record> records = this.db.Records.Where(e => e.Department == d);
            foreach (var item in records)
            {
                item.Department = null;
            }
        }

        /// <summary>
		/// 批量修改档案实体的所属职位属性为null，执行多次sql版本
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public void UpdateRecordOfPositionSetNull(Position p)
        {
            IQueryable<Record> records = this.db.Records.Where(e => e.Position == p);
            foreach (var item in records)
            {
                item.Position = null;
            }
        }

        /// <summary>
        /// 批量修改简历实体的所属部门属性为null，执行多次sql版本
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public void UpdateResumeOfDepartmentSetNull(Department d)
        {
            IQueryable<Resume> resumes = this.db.Resumes.Where(e => e.Department == d);
            foreach (var item in resumes)
            {
                item.Department = null;
            }
        }

        /// <summary>
		/// 批量修改简历实体的所属职位属性为null，执行多次sql版本
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public void UpdateResumeOfPositionSetNull(Position p)
        {
            IQueryable<Resume> resumes = this.db.Resumes.Where(e => e.Position == p);
            foreach (var item in resumes)
            {
                item.Position = null;
            }
        }

        /// <summary>
        /// 根据简历id查找一个简历实体
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns>存在简历id对应的简历实体则返回true</returns>
		public async Task<(bool,string,Resume?)> FindResumeByIdAsync(int? resumeId)
		{
            if (resumeId == null)
            {
                return (false, "简历Id不能为null", null);
            }
            Resume r = await this.db.Resumes.FindAsync(resumeId);
            if (r == null)
            {
                return (false, "没有此简历", null);
            }
            return (true, "查找成功", r);
        }

        /// <summary>
        /// 根据简历id查找一个简历实体，且关联查询招聘需求实体
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns>存在简历id对应的简历实体则返回true</returns>
		public async Task<(bool, string, Resume?)> FindResumeByIdPlusAsync(int? resumeId)
        {
            if (resumeId == null)
            {
                return (false, "简历Id不能为null", null);
            }
            Resume? r = await this.db.Resumes.Include(HiringNeedApply => HiringNeedApply.HiringNeedApply).FirstOrDefaultAsync(Resume => Resume.Id == resumeId);
            if (r == null)
            {
                return (false, "没有此简历", null);
            }
            return (true, "查找成功", r);
        }
    }
}
