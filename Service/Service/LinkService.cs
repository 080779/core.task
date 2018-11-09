﻿using Common;
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
            dto.Name = entity.Name;
            dto.ImgUrl = entity.ImgUrl;
            dto.Url = entity.Url;
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.Sort = entity.Sort;
            dto.IsEnabled = entity.IsEnabled;
            return dto;
        }

        public async Task<long> AddAsync(string name, string imgUrl, string url, int sort)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkEntity entity = new LinkEntity();
                entity.Name = name;
                entity.ImgUrl = imgUrl;
                entity.Url = url;
                entity.Sort = sort;
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
                entity.IsEnabled = entity.IsEnabled == 1 ? 0 : 1;
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
                entity.IsDeleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<LinkDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkEntity entity = await dbc.GetAll<LinkEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if(entity==null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<LinkSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkSearchResult result = new LinkSearchResult();
                var links = dbc.GetAll<LinkEntity>().AsNoTracking();
                if (!string.IsNullOrEmpty(keyword))
                {
                    links = links.Where(a => a.Name.Contains(keyword));
                }
                if (startTime != null)
                {
                    links = links.Where(a => a.CreateTime >= startTime);
                }
                if (endTime != null)
                {
                    links = links.Where(a => a.CreateTime.Year <= endTime.Value.Year && a.CreateTime.Month <= endTime.Value.Month && a.CreateTime.Day <= endTime.Value.Day);
                }
                result.PageCount = (int)Math.Ceiling((await links.LongCountAsync()) * 1.0f / pageSize);
                var linksResult = await links.OrderBy(a => a.Sort).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.Links = linksResult.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }
    }
}