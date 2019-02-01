using Common;
using DTO;
using IService;
using Microsoft.EntityFrameworkCore;
using Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class AdminService : IAdminService
    {
        public AdminDTO ToDTO(AdminEntity entity, long[] permissionIds)
        {
            AdminDTO dto = new AdminDTO();
            dto.CreateTime = entity.CreateTime;
            dto.TrueName = entity.TrueName;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.Mobile = entity.Mobile;
            dto.Enabled = entity.Enabled;
            dto.Remark = entity.Remark;
            dto.PermissionIds = permissionIds;
            return dto;
        }
        public async Task<long> AddAsync(string name, string mobile, string trueName, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                using (var scope = dbc.Database.BeginTransaction())
                {
                    try
                    {
                        long userId = await dbc.GetEntityIdAsync<AdminEntity>(a => a.Name == name);
                        if (userId > 0)
                        {
                            return -1;
                        }
                        userId = await dbc.GetEntityIdAsync<AdminEntity>(a => a.Mobile == mobile);
                        if (userId > 0)
                        {
                            return -2;
                        }

                        AdminEntity entity = new AdminEntity();
                        entity.Name = name;
                        entity.Mobile = mobile;
                        entity.TrueName = trueName;
                        entity.Salt = CommonHelper.GetCaptcha(4);
                        entity.Password = CommonHelper.GetMD5(password + entity.Salt);
                        dbc.Admins.Add(entity);
                        await dbc.SaveChangesAsync();
                        scope.Commit();
                        return entity.Id;
                    }
                    catch (Exception ex)
                    {
                        scope.Rollback();
                        return -3;
                    }
                }                
            }
        }

        public async Task<bool> EditAsync(long id, List<long> permissionIds)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = await dbc.GetAll<AdminEntity>().SingleOrDefaultAsync(a => a.Id == id);
                if (entity == null)
                {
                    return false;
                }
                dbc.AdminPermissions.RemoveRange(dbc.AdminPermissions.Where(a => a.AdminId == id));
                foreach (long perimssionId in permissionIds)
                {
                    dbc.AdminPermissions.Add(new AdminPermissionEntity { AdminId = id, PermissionId = perimssionId });
                }
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> EditAsync(long id, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = await dbc.GetAll<AdminEntity>().SingleOrDefaultAsync(a => a.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.Password = CommonHelper.GetMD5(password + entity.Salt);
                await dbc.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> DelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = await dbc.GetAll<AdminEntity>().SingleOrDefaultAsync(a => a.Id == id);
                if (entity == null)
                {
                    return false;
                }

                entity.Deleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> FrozenAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = await dbc.GetAll<AdminEntity>().SingleOrDefaultAsync(a => a.Id == id);
                if (entity == null)
                {
                    return false;
                }

                entity.Enabled = entity.Enabled == 1 ? 0 : 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<string> GetMobileByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetStringPropertyAsync<AdminEntity>(a => a.Id == id, a => a.Mobile);
            }
        }

        public async Task<string> GetNameByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetStringPropertyAsync<AdminEntity>(a => a.Id == id, a => a.Name);
            }
        }

        public async Task<AdminDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = await dbc.GetAll<AdminEntity>().SingleOrDefaultAsync(a => a.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity, dbc.GetAll<AdminPermissionEntity>().Where(a => a.AdminId == id).Select(a => a.PermissionId).ToArray());
            }
        }

        public async Task<AdminSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                AdminSearchResult result = new AdminSearchResult();
                var entities = dbc.GetAll<AdminEntity>().AsNoTracking();
                if (!string.IsNullOrEmpty(keyword))
                {
                    entities = entities.Where(a => a.Name.Contains(keyword));
                }
                if (startTime != null)
                {
                    entities = entities.Where(a => a.CreateTime >= startTime);
                }
                if (endTime != null)
                {
                    entities = entities.Where(a => a.CreateTime.Year <= endTime.Value.Year && a.CreateTime.Month <= endTime.Value.Month && a.CreateTime.Day <= endTime.Value.Day);
                }
                result.PageCount = (int)Math.Ceiling((await entities.LongCountAsync()) * 1.0f / pageSize);
                var res = await entities.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.List = res.Select(a => ToDTO(a, dbc.GetAll<AdminPermissionEntity>().Where(ap => ap.AdminId == a.Id).Select(ap => ap.PermissionId).ToArray())).ToArray();
                return result;
            }
        }

        
        public async Task<KeyValuePair<bool, string>> CheckPermAsync(long adminId, string typeRemark, string remark)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                PermissionEntity perm = await dbc.GetAll<PermissionEntity>().AsNoTracking().SingleOrDefaultAsync(p => p.TypeRemark==typeRemark && p.Remark == remark);
                if(perm==null)
                {
                    return new KeyValuePair<bool, string>(false,null);
                }
                long res = await dbc.GetEntityIdAsync<AdminPermissionEntity>(a => a.AdminId == adminId && a.PermissionId == perm.Id);
                if(res<=0)
                {
                    return new KeyValuePair<bool, string>(false, perm.Name);
                }
                return new KeyValuePair<bool, string>(true, perm.Name);
            }
        }

        public async Task<long> CheckLoginAsync(string name, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = await dbc.GetAll<AdminEntity>().SingleOrDefaultAsync(a => a.Name == name);
                if (entity == null)
                {
                    return -1;
                }
                string pwd = CommonHelper.GetMD5(password + entity.Salt);
                if (entity.Password != pwd)
                {
                    return -2;
                }
                if (entity.Enabled == 0)
                {
                    return -3;
                }                
                return entity.Id;
            }
        }

        public async Task<bool> DelAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                //await dbc.Database.ExecuteSqlCommandAsync("exec del_all");
                var res = dbc.Notices.FromSql("select * from tb_notices");
                var count = await res.CountAsync();
                return true;
            }
        }
    }
}
