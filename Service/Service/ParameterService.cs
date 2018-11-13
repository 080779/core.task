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
    public class ParameterService : IParameterService
    {
        private ParameterDTO ToDTO(ParameterEntity entity)
        {
            if(entity==null)
            {
                return null;
            }
            ParameterDTO dto = new ParameterDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.Remark = entity.Remark;
            dto.Sort = entity.Sort;
            dto.IsEnabled = entity.IsEnabled;
            //dto.TypeName = entity.Type.Name;
            dto.DecimalValue = entity.DecimalValue;
            dto.StringValue = entity.StringValue;
            return dto;
        }

        private ParameterSettingDTO ToDTO(string typeName, ParameterDTO[] parameters)
        {
            ParameterSettingDTO dto = new ParameterSettingDTO();
            dto.TypeName = typeName;
            dto.Parameters = parameters;
            return dto;
        }

        public async Task<long> AddAsync(string name,string stringValue,decimal decimalValue, string remark, int sort, long typeId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                string typeName = await dbc.GetParameterAsync<ParameterTypeEntity>(p => p.Id == typeId, p => p.Name);
                if (string.IsNullOrEmpty(typeName))
                {
                    return -1;
                }
                ParameterEntity entity = new ParameterEntity();
                entity.Name = name;
                entity.Remark = remark;
                entity.Sort = sort;
                entity.TypeId = typeId;
                entity.StringValue = stringValue;
                entity.DecimalValue = decimalValue;
                dbc.Parameters.Add(entity);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<long> EditAsync(long id, string name, string stringValue, decimal decimalValue, string remark, int sort)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ParameterEntity entity = await dbc.GetAll<ParameterEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return -1;
                }
                entity.Name = name;
                entity.Remark = remark;
                entity.Sort = sort;
                entity.StringValue = stringValue;
                entity.DecimalValue = decimalValue;
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<bool> FrozenAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ParameterEntity entity = await dbc.GetAll<ParameterEntity>().SingleOrDefaultAsync(p => p.Id == id);
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
                ParameterEntity entity = await dbc.GetAll<ParameterEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsDeleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<ParameterDTO> GetModelByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = await dbc.GetAll<ParameterEntity>().Include(p => p.Type).AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<ParameterDTO[]> GetByTypeIdIsEnableAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<ParameterEntity>().Include(p => p.Type).AsNoTracking().Where(p => p.TypeId == id && p.IsEnabled == 1);
                var idNames = await entities.OrderBy(p => p.Sort).ToListAsync();
                return idNames.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<ParameterDTO[]> GetByTypeIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<ParameterEntity>().Include(p => p.Type).AsNoTracking().Where(p => p.TypeId == id);
                var idNames = await entities.OrderBy(p => p.Sort).ToListAsync();
                return idNames.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<ParameterSettingDTO[]> GetAllIsEnableAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<ParameterTypeEntity>().AsNoTracking().Where(p => p.IsEnabled == 1);
                var parameterSettings = await entities.OrderBy(p => p.Sort).ToListAsync();
                return parameterSettings.Select(p => ToDTO(p.Name,dbc.GetAll<ParameterEntity>().AsNoTracking().Where(pp=>pp.TypeId==p.Id).ToList().Select(pp=>ToDTO(pp)).ToArray())).ToArray();
            }
        }
    }
}
