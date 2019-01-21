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
    public class AdminLogService : IAdminLogService
    {
        public AdminLogDTO ToDTO(AdminLogEntity entity)
        {
            AdminLogDTO dto = new AdminLogDTO();
            dto.AdminId = entity.AdminId;
            dto.AdminMobile = entity.AdminMobile;
            dto.CreateTime = entity.CreateTime;
            dto.Remark = entity.Remark;
            dto.Id = entity.Id;
            dto.IpAddress = entity.IpAddress;
            dto.PermTypeName = entity.PermTypeName;
            dto.Tip = entity.Tip;
            return dto;
        }
        public async Task<long> AddAsync(long adminId, string permTypeName, string remark, string ipAddress, string tip)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                AdminLogEntity entity = new AdminLogEntity();
                entity.AdminId = adminId;
                entity.AdminMobile = await dbc.GetStringPropertyAsync<AdminEntity>(a => a.Id == adminId,a=>a.Mobile);
                entity.PermTypeName = permTypeName;
                entity.Remark = remark;
                entity.IpAddress = ipAddress;
                entity.Tip = tip;
                dbc.AdminLogs.Add(entity);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<AdminLogSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                AdminLogSearchResult result = new AdminLogSearchResult();
                var entities = dbc.GetAll<AdminLogEntity>().AsNoTracking();
                if (!string.IsNullOrEmpty(keyword))
                {
                    entities = entities.Where(a => a.AdminMobile.Contains(keyword));
                }
                if (startTime != null)
                {
                    entities = entities.Where(a => a.CreateTime >= startTime);
                }
                if (endTime != null)
                {
                    entities = entities.Where(a => a.CreateTime <= endTime);
                }
                result.PageCount = (int)Math.Ceiling((await entities.LongCountAsync()) * 1.0f / pageSize);
                var res = await entities.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.List = res.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }
    }
}
