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
            dto.Id = entity.Id;
            dto.Enabled = entity.Enabled;
            dto.LevelId = entity.LevelId;
            dto.Name = entity.Name;
            dto.Remark = entity.Remark;
            dto.TypeName = entity.TypeName;
            dto.TypeRemark = entity.TypeRemark;
            dto.Url = entity.Url;
            dto.Icon = entity.Icon;
            dto.Sort = entity.Sort;
            return dto;
        }

        public async Task InitializeAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var dataList = await dbc.GetAll<PermissionEntity>().ToListAsync();
                foreach (var item in dataList)
                {
                    item.Enabled = 0;
                    item.Icon = null;
                    item.Sort = null;
                }
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
                    if(!string.IsNullOrEmpty(url))
                    {
                        entity.Icon = "icon-cogs";
                        entity.Sort = 1;
                    }
                    entity.LevelId = levelId;
                    entity.Enabled = 1;
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
                    if (!string.IsNullOrEmpty(url))
                    {
                        entity.Icon = "icon-cogs";
                        entity.Sort = 1;
                    }
                    entity.LevelId = levelId;
                    dbc.Permissions.Add(entity);
                    await dbc.SaveChangesAsync();
                    return entity.Id;
                }                
            }
        }        

        public async Task<string[]> GetModelTypeListIsEnableAsync()
        {
            using (MyDbContext dbc=new MyDbContext())
            {
                var res = await dbc.GetAll<PermissionEntity>().AsNoTracking().Where(p => p.Enabled == 1 && p.LevelId==0).OrderBy(p=>p.Sort).ToListAsync();
                return res.Select(p => p.TypeName).ToArray();
            }
        }

        public async Task<PermissionDTO[]> GetModelListIsEnableByTypeNameAsync(string typeName)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var res = await dbc.GetAll<PermissionEntity>().AsNoTracking().Where(p => p.Enabled == 1 && p.TypeName == typeName).ToListAsync();
                return res.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<PermissionDTO[]> GetModelUrlListIsEnableAsync()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var res = await dbc.GetAll<PermissionEntity>().AsNoTracking().Where(p => p.Enabled == 1 && p.LevelId==0).OrderBy(p=>p.Sort).ToListAsync();
                return res.Select(p => ToDTO(p)).ToArray();
            }
        }
    }
}
