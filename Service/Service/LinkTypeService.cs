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
    public class LinkTypeService : ILinkTypeService
    {
        private LinkTypeDTO ToDTO(LinkTypeEntity entity)
        {
            LinkTypeDTO dto = new LinkTypeDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.Description = entity.Description;
            dto.IsEnabled = entity.IsEnabled;
            return dto;
        }

        public async Task<long> AddAsync(string name, string description)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkTypeEntity entity = new LinkTypeEntity();
                entity.Name = name;
                entity.Description = description;
                dbc.LinkTypes.Add(entity);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<long> EditAsync(long id, string name, string description)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkTypeEntity entity = await dbc.GetAll<LinkTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return -1;
                }
                entity.Name = name;
                entity.Description = description;
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<bool> FrozenAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkTypeEntity entity = await dbc.GetAll<LinkTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
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
                LinkTypeEntity entity = await dbc.GetAll<LinkTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsDeleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<LinkTypeDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkTypeEntity entity = await dbc.GetAll<LinkTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<LinkTypeDTO[]> GetModelListIsEnableAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<LinkTypeEntity>().AsNoTracking().Where(p => p.IsEnabled == 1);
                var linkTypes = await entities.ToListAsync();
                return linkTypes.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<LinkTypeDTO[]> GetModelListAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<LinkTypeEntity>().AsNoTracking();
                var linkTypes = await entities.ToListAsync();
                return linkTypes.Select(p => ToDTO(p)).ToArray();
            }
        }
    }
}
