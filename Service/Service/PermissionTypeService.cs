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
            return dto;
        }

        public async Task<bool> DelByNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                PermissionTypeEntity permissionType = await dbc.GetAll<PermissionTypeEntity>().SingleOrDefaultAsync(i => i.Name == name);
                if (permissionType == null)
                {
                    return false;
                }
                permissionType.IsDeleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<PermissionTypeDTO[]> GetModelList()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<PermissionTypeEntity>().AsNoTracking().Where(p=>p.IsEnabled == 1);
                var permissionTypes = await entities.ToListAsync();
                return permissionTypes.Select(p => ToDTO(p)).ToArray();
            }
        }
    }
}
