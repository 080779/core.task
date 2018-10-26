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
    public class PermissionService : IPermissionService
    {
        public PermissionDTO ToDTO(PermissionEntity entity)
        {
            PermissionDTO dto = new PermissionDTO();
            dto.Description = entity.Description;
            dto.Name = entity.Name;
            dto.PermissionTypeId = entity.PermissionTypeId;
            dto.PermissionTypeName = entity.PermissionType.Name;
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.Sort = entity.Sort;
            dto.IsEnabled = entity.IsEnabled;
            return dto;
        }

        public async Task<long> AddAsync(string name, int sort, long permissionTypeId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                string typeName = await dbc.GetParameterAsync<PermissionTypeEntity>(p=>p.Id==permissionTypeId,p=>p.Name);
                if(string.IsNullOrEmpty(typeName))
                {
                    return -1;
                }
                PermissionEntity entity = new PermissionEntity();
                entity.Name = name;
                entity.Description = typeName + "_" + name;
                entity.Sort = sort;
                entity.PermissionTypeId = permissionTypeId;
                dbc.Permissions.Add(entity);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<long> EditAsync(long id, string name, int sort)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                PermissionEntity entity = await dbc.GetAll<PermissionEntity>().SingleOrDefaultAsync(p=>p.Id==id);
                if (entity==null)
                {
                    return -1;
                }
                entity.Name = name;
                entity.Description = entity.Description.Split('_')[0] + "_" + name;
                entity.Sort = sort;
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<bool> FrozenAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                PermissionEntity entity = await dbc.GetAll<PermissionEntity>().SingleOrDefaultAsync(p => p.Id == id);
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
                PermissionEntity entity = await dbc.GetAll<PermissionEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsDeleted = 1;
                return true;
            }
        }

        public async Task<PermissionDTO> GetModelByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = await dbc.GetAll<PermissionEntity>().Include(p => p.PermissionType).AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);
                if(entity==null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public PermissionDTO GetByDesc(string description)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = dbc.GetAll<PermissionEntity>().Include(p => p.PermissionType).AsNoTracking().SingleOrDefault(p => p.Description == description);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<PermissionDTO[]> GetByTypeIdIsEnableAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<PermissionEntity>().Include(p => p.PermissionType).AsNoTracking().Where(p => p.PermissionTypeId == id && p.IsEnabled==1);
                var permissions = await entities.ToListAsync();
                return permissions.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<PermissionDTO[]> GetByTypeIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<PermissionEntity>().Include(p => p.PermissionType).AsNoTracking().Where(p => p.PermissionTypeId == id);
                var permissions = await entities.ToListAsync();
                return permissions.Select(p => ToDTO(p)).ToArray();
            }
        }
    }
}
