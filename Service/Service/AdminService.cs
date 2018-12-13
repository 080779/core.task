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
            dto.IsEnabled = entity.IsEnabled;
            dto.PermissionIds = permissionIds;
            return dto;
        }
        public async Task<long> AddAsync(string name, string mobile, string trueName, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                long userId = await dbc.GetIdAsync<AdminEntity>(a => a.Name == name);
                if (userId > 0)
                {
                    return -1;
                }
                userId = await dbc.GetIdAsync<AdminEntity>(a => a.Mobile == mobile);
                if(userId>0)
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
                return entity.Id;
            }
        }
        public async Task<bool> EditAsync(long id, string mobile, string trueName, string password, long[] permissionIds)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = await dbc.GetAll<AdminEntity>().SingleOrDefaultAsync(a => a.Id == id);
                if (entity == null)
                {
                    return false;
                }

                entity.Mobile = mobile;
                entity.TrueName = trueName;
                entity.Password = CommonHelper.GetMD5(password + entity.Salt);
                dbc.AdminPermissions.RemoveRange(dbc.AdminPermissions.Where(a => a.AdminId == id));
                foreach (long perimssionId in permissionIds)
                {
                    dbc.AdminPermissions.Add(new AdminPermissionEntity { AdminId = id, PermissionId = perimssionId });
                }
                await dbc.SaveChangesAsync();
                return true;
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

                entity.IsDeleted = 1;
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

                entity.IsEnabled = entity.IsEnabled == 1 ? 0 : 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<string> GetMobileByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetParameterAsync<AdminEntity>(a => a.Id == id, a => a.Mobile);
            }
        }

        public async Task<string> GetNameByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetParameterAsync<AdminEntity>(a => a.Id == id, a => a.Name);
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

        public async Task<AdminSearchResult> GetModelListAsync(string isAdmin, string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                AdminSearchResult result = new AdminSearchResult();
                var admins = dbc.GetAll<AdminEntity>().AsNoTracking();
                if (isAdmin != "admin")
                {
                    admins = admins.Where(a => a.Name == isAdmin);
                }
                else
                {
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        admins = admins.Where(a => a.Name.Contains(keyword));
                    }
                    if (startTime != null)
                    {
                        admins = admins.Where(a => a.CreateTime >= startTime);
                    }
                    if (endTime != null)
                    {
                        admins = admins.Where(a => a.CreateTime.Year <= endTime.Value.Year && a.CreateTime.Month <= endTime.Value.Month && a.CreateTime.Day <= endTime.Value.Day);
                    }
                }
                result.PageCount = (int)Math.Ceiling((await admins.LongCountAsync()) * 1.0f / pageSize);
                var adminsResult = await admins.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.Admins = adminsResult.Select(a => ToDTO(a, dbc.GetAll<AdminPermissionEntity>().Where(ap => ap.AdminId == a.Id).Select(ap => ap.PermissionId).ToArray())).ToArray();
                return result;
            }
        }

        public async Task<AdminSearchResult> GetModelListHasPerAsync(string isAdmin, string mobile, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                AdminSearchResult result = new AdminSearchResult();
                var admins = dbc.GetAll<AdminEntity>().AsNoTracking();
                if (isAdmin != "admin")
                {
                    admins = admins.Where(a => a.Mobile != "admin");
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    admins = admins.Where(a => a.Mobile.Contains(mobile));
                }
                if (startTime != null)
                {
                    admins = admins.Where(a => a.CreateTime.Year >= startTime.Value.Year && a.CreateTime.Month >= startTime.Value.Month && a.CreateTime.Day >= startTime.Value.Day);
                }
                if (endTime != null)
                {
                    admins = admins.Where(a => a.CreateTime <= endTime);
                }
                result.PageCount = (int)Math.Ceiling((await admins.LongCountAsync()) * 1.0f / pageSize);
                var adminsResult = await admins.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.Admins = adminsResult.Select(a => ToDTO(a, dbc.GetAll<AdminPermissionEntity>().Where(ap => ap.AdminId == a.Id).Select(ap => ap.PermissionId).ToArray())).ToArray();
                return result;
            }
        }

        public bool HasPermission(long id, string description)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return dbc.GetAll<AdminPermissionEntity>().Include(a => a.Permission).Any(a => a.AdminId == id && a.Permission.Description == description);
            }
        }

        public async Task<long> CheckLogin(string mobile, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var admin = await dbc.GetAll<AdminEntity>().SingleOrDefaultAsync(a => a.Mobile == mobile);
                if (admin == null)
                {
                    return -1;
                }
                if (admin.IsEnabled==0)
                {
                    return -2;
                }
                string pwd = CommonHelper.GetMD5(password + admin.Salt);
                if (admin.Password != pwd)
                {
                    return -3;
                }
                return admin.Id;
            }
        }

        public async Task<bool> DelAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                await dbc.Database.ExecuteSqlCommandAsync("exec del_all");
                return true;
            }
        }
    }
}
