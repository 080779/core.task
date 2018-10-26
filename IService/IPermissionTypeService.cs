using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IPermissionTypeService:IServiceSupport
    {
        Task<long> AddAsync(string name,string description,int sort);
        Task<long> EditAsync(long id,string name, string description, int sort);
        Task<bool> FrozenAsync(long id);
        Task<bool> DelAsync(long id);
        Task<PermissionTypeDTO> GetModelAsync(long id);
        Task<PermissionTypeDTO[]> GetModelListIsEnableAsync();
        Task<PermissionTypeDTO[]> GetModelListAsync();
    }
}
