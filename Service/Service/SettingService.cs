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
    public class SettingService : ISettingService
    {
        private SettingDTO ToDTO(SettingEntity entity)
        {
            SettingDTO dto = new SettingDTO();
            dto.Id = entity.Id;
            dto.Parameter = entity.Parameter;
            dto.Name = entity.Name;
            dto.TypeId = entity.TypeId;
            return dto;
        }


        public async Task<string> GetParmByNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetStringPropertyAsync<SettingEntity>(s=>s.Name==name,s=>s.Parameter);                 
            }
        }

        public async Task<SettingDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                SettingEntity entity = await dbc.GetAll<SettingEntity>().AsNoTracking().SingleOrDefaultAsync(g => g.Id == id);
                if (entity == null)
                {
                    return null;
                }                
                return ToDTO(entity);
            }
        }

        public async Task<SettingDTO> GetModelByNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                SettingEntity entity = await dbc.GetAll<SettingEntity>().AsNoTracking().SingleOrDefaultAsync(g => g.Name == name);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }
        
        public async Task<SettingDTO[]> GetModelListByDescAsync(string desc)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                List<SettingEntity> settings = await dbc.GetAll<SettingEntity>().AsNoTracking().Where(g => g.Description == desc).ToListAsync();
                return settings.Select(s => ToDTO(s)).ToArray();
            }
        }

        public async Task<bool> UpdateAsync(long id, string parameter)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                SettingEntity entity = await dbc.GetAll<SettingEntity>().SingleOrDefaultAsync(g => g.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.Parameter = parameter;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateAsync(params SettingDTO[] settings)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                foreach(SettingDTO setting in settings)
                {
                    SettingEntity entity = await dbc.GetAll<SettingEntity>().SingleOrDefaultAsync(g => g.Id == setting.Id);
                    if (entity == null)
                    {
                        return false;
                    }
                    entity.Parameter = setting.Parameter.ToString();
                }
                await dbc.SaveChangesAsync();
                return true;
            }
        }
    }
}
