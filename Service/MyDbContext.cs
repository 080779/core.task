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
            optionsBuilder.UseMySQL("Server=18.191.6.120;database=db_task;uid=root;pwd=root;characterset=utf8");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public IQueryable<T> GetAll<T>() where T : BaseEntity
        {
            return this.Set<T>().Where(e => e.IsDeleted == 0);
        }

        public long GetId<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == 0).Where(expression).Select(e => e.Id).SingleOrDefault();
        }

        public async Task<long> GetIdAsync<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == 0).Where(expression).Select(e => e.Id).SingleOrDefaultAsync();
        }

        public IQueryable<long> GetIds<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == 0).Where(expression).Select(e => e.Id);
        }

        public string GetParameter<T>(Expression<Func<T, bool>> expression, Expression<Func<T, string>> parameterName) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == 0).Where(expression).Select(parameterName).SingleOrDefault();
        }

        public async Task<string> GetParameterAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, string>> parameterName) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == 0).Where(expression).Select(parameterName).SingleOrDefaultAsync();
        }

        public long GetlongParameter<T>(Expression<Func<T, bool>> expression, Expression<Func<T, long>> parameterName) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == 0).Where(expression).Select(parameterName).SingleOrDefault();
        }

        public async Task<long> GetlongParameterAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, long>> parameterName) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == 0).Where(expression).Select(parameterName).SingleOrDefaultAsync();
        }

        public decimal GetDecimalParameter<T>(Expression<Func<T, bool>> expression, Expression<Func<T, decimal>> parameterName) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == 0).Where(expression).Select(parameterName).SingleOrDefault();
        }

        public async Task<decimal> GetDecimalParameterAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, decimal>> parameterName) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == 0).Where(expression).Select(parameterName).SingleOrDefaultAsync();
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AdminEntity> Admins { get; set; }
        public DbSet<AdminLogEntity> AdminLogs { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<PermissionTypeEntity> PermissionTypes { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<TakeCashEntity> TakeCashes { get; set; }
        public DbSet<IdNameEntity> IdNames { get; set; }
        public DbSet<JournalEntity> Journals { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<ForwardEntity> Forwards { get; set; }
        public DbSet<ForwardStateEntity> ForwardStates { get; set; }
        public DbSet<CollectEntity> Collects { get; set; }
        public DbSet<AdminPermissionEntity> AdminPermissions { get; set; }
        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<IdNameTypeEntity> IdNameTypes { get; set; }
    }
}
