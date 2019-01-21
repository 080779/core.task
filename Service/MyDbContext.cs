using Microsoft.EntityFrameworkCore;
using Service.Config;
using Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MyDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseMySQL("Server=18.191.6.120;database=db_task;uid=root;pwd=root;characterset=utf8"); //aws_mysql服务器
            optionsBuilder.UseMySQL("Server=123.207.5.234;database=db_task;uid=root;pwd=root;characterset=utf8"); //tx_mysql服务器
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //自动应用config配置
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        #region 实体类集合
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AdminEntity> Admins { get; set; }
        public DbSet<AdminLogEntity> AdminLogs { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<TakeCashEntity> TakeCashes { get; set; }
        public DbSet<JournalEntity> Journals { get; set; }
        public DbSet<AdminPermissionEntity> AdminPermissions { get; set; }
        public DbSet<LinkEntity> Links { get; set; }
        public DbSet<NoticeEntity> Notices { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        #endregion

        #region MyDbContext通用方法
        public IQueryable<T> GetAll<T>() where T : BaseEntity
        {
            return this.Set<T>().Where(e => e.Deleted == 0 || e.Deleted==null);
        }

        public long GetEntityId<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.Deleted == 0 || e.Deleted == null).Where(expression).Select(e => e.Id).SingleOrDefault();
        }

        public async Task<long> GetEntityIdAsync<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.Deleted == 0 || e.Deleted == null).Where(expression).Select(e => e.Id).SingleOrDefaultAsync();
        }

        public IQueryable<long> GetEntityIds<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.Deleted == 0 || e.Deleted == null).Where(expression).Select(e => e.Id);
        }

        public string GetStringProperty<T>(Expression<Func<T, bool>> expression, Expression<Func<T, string>> parameterName) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.Deleted == 0 || e.Deleted == null).Where(expression).Select(parameterName).SingleOrDefault();
        }

        public async Task<string> GetStringPropertyAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, string>> parameterName) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.Deleted == 0 || e.Deleted == null).Where(expression).Select(parameterName).SingleOrDefaultAsync();
        }

        public decimal GetDecimalProperty<T>(Expression<Func<T, bool>> expression, Expression<Func<T, decimal>> parameterName) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.Deleted == 0 || e.Deleted == null).Where(expression).Select(parameterName).SingleOrDefault();
        }

        public async Task<decimal> GetDecimalPropertyAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, decimal>> parameterName) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.Deleted == 0 || e.Deleted == null).Where(expression).Select(parameterName).SingleOrDefaultAsync();
        }

        public int GetIntProperty<T>(Expression<Func<T, bool>> expression, Expression<Func<T, int>> parameterName) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.Deleted == 0 || e.Deleted == null).Where(expression).Select(parameterName).SingleOrDefault();
        }

        public async Task<int> GetIntPropertyAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, int>> parameterName) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.Deleted == 0 || e.Deleted == null).Where(expression).Select(parameterName).SingleOrDefaultAsync();
        }
        #endregion
    }
}
