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
    public class PermissionTypeService : IPermissionTypeService
    {
        public PermissionTypeDTO ToDTO(PermissionTypeEntity entity)
        {
            PermissionTypeDTO dto = new PermissionTypeDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Description = entity.Description;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.Sort = entity.Sort;
            dto.IsEnabled = entity.IsEnabled;
            return dto;
        }

        public async Task<long> AddAsync(string name, string description, int sort)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                PermissionTypeEntity entity = new PermissionTypeEntity();
                entity.Name = name;
                entity.Description = description;
                entity.Sort = sort;
                dbc.PermissionTypes.Add(entity);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<long> EditAsync(long id, string name, string description, int sort)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                PermissionTypeEntity entity = await dbc.GetAll<PermissionTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if(entity==null)
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
                PermissionTypeEntity entity = await dbc.GetAll<PermissionTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
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
                PermissionTypeEntity entity = await dbc.GetAll<PermissionTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsDeleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<PermissionTypeDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                PermissionTypeEntity entity = await dbc.GetAll<PermissionTypeEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<PermissionTypeDTO[]> GetModelListIsEnableAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<PermissionTypeEntity>().AsNoTracking().Where(p => p.IsEnabled == 1);
                var permissionTypes = await entities.OrderBy(p => p.Sort).ToListAsync();
                return permissionTypes.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<PermissionTypeDTO[]> GetModelListAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<PermissionTypeEntity>().AsNoTracking();
                var permissionTypes = await entities.OrderBy(p=>p.Sort).ToListAsync();
                return permissionTypes.Select(p => ToDTO(p)).ToArray();
            }
        }
    }
}
