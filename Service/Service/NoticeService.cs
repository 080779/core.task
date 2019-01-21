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
    public class NoticeService : INoticeService
    {
        public NoticeDTO ToDTO(NoticeEntity entity)
        {
            NoticeDTO dto = new NoticeDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Enabled = entity.Enabled;
            dto.Content = entity.Content;
            dto.Creator = entity.Creator;
            dto.FailureTime = entity.FailureTime;
            dto.Id = entity.Id;
            dto.Tip = entity.Tip;
            dto.Title = entity.Title;
            dto.Url = entity.Url;
            return dto;
        }

        public async Task<long> AddAsync(string title, string content, int enabled, long adminId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                NoticeEntity entity = new NoticeEntity();
                entity.Title = title;
                entity.Content = content;
                entity.Enabled = enabled;
                entity.Creator = await dbc.GetStringPropertyAsync<AdminEntity>(a => a.Id == adminId, a => a.Name);
                dbc.Notices.Add(entity);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<bool> EditAsync(long id, string title, string content, int enabled)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                NoticeEntity entity = await dbc.GetAll<NoticeEntity>().SingleOrDefaultAsync(n=>n.Id==id);
                if(entity==null)
                {
                    return false;
                }
                entity.Title = title;
                entity.Content = content;
                entity.Enabled = enabled;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                NoticeEntity entity = await dbc.GetAll<NoticeEntity>().SingleOrDefaultAsync(p => p.Id == id);
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
                NoticeEntity entity = await dbc.GetAll<NoticeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.Enabled = 0;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<NoticeDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                NoticeEntity entity = await dbc.GetAll<NoticeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<NoticeSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                NoticeSearchResult result = new NoticeSearchResult();
                var entities = dbc.GetAll<NoticeEntity>().AsNoTracking();
                if (!string.IsNullOrEmpty(keyword))
                {
                    entities = entities.Where(a => a.Title.Contains(keyword));
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
                result.List = res.Select(a => ToDTO(a)).ToArray();
                return result;
            }                
        }
    }
}
