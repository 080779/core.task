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
    public class ParameterTypeService : IParameterTypeService
    {
        private ParameterTypeDTO ToDTO(ParameterTypeEntity entity)
        {
            ParameterTypeDTO dto = new ParameterTypeDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.Description = entity.Description;
            dto.IsEnabled = entity.IsEnabled;
            dto.Sort = entity.Sort;
            return dto;
        }

        public async Task<long> AddAsync(string name, string description, int sort)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ParameterTypeEntity entity = new ParameterTypeEntity();
                entity.Name = name;
                entity.Description = description;
                entity.Sort = sort;
                dbc.ParameterTypes.Add(entity);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<long> EditAsync(long id, string name, string description, int sort)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ParameterTypeEntity entity = await dbc.GetAll<ParameterTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return -1;
                }
                entity.Name = name;
                entity.Description = description;
                entity.Sort = sort;
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<bool> FrozenAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ParameterTypeEntity entity = await dbc.GetAll<ParameterTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
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
                ParameterTypeEntity entity = await dbc.GetAll<ParameterTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsDeleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<ParameterTypeDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ParameterTypeEntity entity = await dbc.GetAll<ParameterTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<ParameterTypeDTO[]> GetModelListIsEnableAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<ParameterTypeEntity>().AsNoTracking().Where(p => p.IsEnabled == 1);
                var idNameTypes = await entities.ToListAsync();
                return idNameTypes.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<ParameterTypeDTO[]> GetModelListAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<ParameterTypeEntity>().AsNoTracking();
                var idNameTypes = await entities.ToListAsync();
                return idNameTypes.Select(p => ToDTO(p)).ToArray();
            }
        }
    }
}
