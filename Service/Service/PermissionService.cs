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
    public class PermissionService : IPermissionService
    {
        public PermissionDTO ToDTO(PermissionEntity entity)
        {
            PermissionDTO dto = new PermissionDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.IsEnabled = entity.IsEnabled;
            dto.LevelId = entity.LevelId;
            dto.Name = entity.Name;
            dto.Remark = entity.Remark;
            dto.TypeName = entity.TypeName;
            dto.TypeRemark = entity.TypeRemark;
            dto.Url = entity.Url;
            return dto;
        }

        public async Task InitializeAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                await dbc.GetAll<PermissionEntity>().ForEachAsync(p => p.IsEnabled = 0);
                await dbc.SaveChangesAsync();
            }
        }

        public async Task<long> EditAsync(long id, string name, string remark, string typeName, string typeRemark, string url, int? levelId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {

                PermissionEntity entity = await dbc.GetAll<PermissionEntity>().SingleOrDefaultAsync(p=>p.Id==id);
                if (entity != null)
                {
                    entity.Name = name;
                    entity.Remark = remark;
                    entity.TypeName = typeName;
                    entity.TypeRemark = typeRemark;
                    entity.Url = url;
                    entity.LevelId = levelId;
                    entity.IsEnabled = 1;
                    await dbc.SaveChangesAsync();
                    return entity.Id;
                }
                else
                {
                    entity = new PermissionEntity();
                    entity.Name = name;
                    entity.Remark = remark;
                    entity.TypeName = typeName;
                    entity.TypeRemark = typeRemark;
                    entity.Url = url;
                    entity.LevelId = levelId;
                    dbc.Permissions.Add(entity);
                    await dbc.SaveChangesAsync();
                    return entity.Id;
                }                
            }
        }        
    }
}
