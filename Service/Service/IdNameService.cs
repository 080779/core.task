﻿using DTO;
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
    public class IdNameService : IIdNameService
    {
        private IdNameDTO ToDTO(IdNameEntity entity)
        {
            IdNameDTO dto = new IdNameDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.Description = entity.Description;
            dto.Sort = entity.Sort;
            dto.IsEnabled = entity.IsEnabled;
            dto.TypeName = entity.IdNameType.Name;
            dto.TypeId = entity.IdNameTypeId;
            return dto;
        }
        
        public async Task<long> AddAsync(string name, string description, int sort, long typeId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                string typeName = await dbc.GetParameterAsync<IdNameTypeEntity>(p => p.Id == typeId, p => p.Name);
                if (string.IsNullOrEmpty(typeName))
                {
                    return -1;
                }
                IdNameEntity entity = new IdNameEntity();
                entity.Name = name;
                entity.Description = description;
                entity.Sort = sort;
                entity.IdNameTypeId = typeId;
                dbc.IdNames.Add(entity);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<long> EditAsync(long id, string name, string description, int sort)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                IdNameEntity entity = await dbc.GetAll<IdNameEntity>().SingleOrDefaultAsync(p => p.Id == id);
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
                IdNameEntity entity = await dbc.GetAll<IdNameEntity>().SingleOrDefaultAsync(p => p.Id == id);
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
                IdNameEntity entity = await dbc.GetAll<IdNameEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsDeleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IdNameDTO> GetModelByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entity = await dbc.GetAll<IdNameEntity>().Include(p => p.IdNameType).AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<IdNameDTO[]> GetByTypeIdIsEnableAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<IdNameEntity>().Include(p => p.IdNameType).AsNoTracking().Where(p => p.IdNameTypeId == id && p.IsEnabled == 1);
                var idNames = await entities.OrderBy(p => p.Sort).ToListAsync();
                return idNames.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<IdNameDTO[]> GetByTypeIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<IdNameEntity>().Include(p => p.IdNameType).AsNoTracking().Where(p => p.IdNameTypeId == id);
                var idNames = await entities.OrderBy(p => p.Sort).ToListAsync();
                return idNames.Select(p => ToDTO(p)).ToArray();
            }
        }
    }
}
