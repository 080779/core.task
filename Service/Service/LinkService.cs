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
    public class LinkService : ILinkService
    {
        public LinkDTO ToDTO(LinkEntity entity)
        {
            LinkDTO dto = new LinkDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Name = entity.Name;
            dto.ImgUrl = entity.ImgUrl;
            dto.Url = entity.Url;
            dto.Id = entity.Id;
            dto.Sort = entity.Sort;
            dto.TypeName = "";
            dto.Enabled = entity.Enabled;
            return dto;
        }

        public async Task<long> AddAsync(int typeId, string name, string imgUrl, string url, int sort)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkEntity entity = new LinkEntity();
                entity.Name = name;
                entity.ImgUrl = imgUrl;
                entity.Url = url;
                entity.Sort = sort;
                entity.TypeId = typeId;
                dbc.Links.Add(entity);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<long> EditAsync(long id, string name, string imgUrl, string url, int sort)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkEntity entity = await dbc.GetAll<LinkEntity>().SingleOrDefaultAsync(p=>p.Id==id);
                if (entity==null)
                {
                    return -1;
                }
                entity.Name = name;
                entity.ImgUrl = imgUrl;
                entity.Url = url;
                entity.Sort = sort;
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<bool> FrozenAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkEntity entity = await dbc.GetAll<LinkEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.Enabled = entity.Enabled == 1 ? 0 : 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkEntity entity = await dbc.GetAll<LinkEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.Deleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<LinkDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkEntity entity = await dbc.GetAll<LinkEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<LinkDTO[]> GetModelListIsEnableAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<LinkEntity>().AsNoTracking().Where(p => p.Enabled == 1);
                var idNames = await entities.OrderBy(p => p.Sort).ToListAsync();
                return idNames.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<LinkDTO[]> GetModelListAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<LinkEntity>().AsNoTracking();
                var idNames = await entities.OrderBy(p => p.Sort).ToListAsync();
                return idNames.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<LinkSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkSearchResult result = new LinkSearchResult();
                var entities = dbc.GetAll<LinkEntity>().AsNoTracking();
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
                var res = await entities.OrderBy(a => a.Sort).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.List = res.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }
    }
}
