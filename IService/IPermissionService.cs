using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IPermissionService : IServiceSupport
    {
        Task<long> AddAsync(string name, int sort, long permissionTypeId);
        Task<long> EditAsync(long id, string name, int sort);
        Task<bool> FrozenAsync(long id);
        Task<bool> DelAsync(long id);
        Task<PermissionDTO> GetModelByIdAsync(long id);
        string GetNameByDesc(string description);
        Task<PermissionDTO[]> GetByTypeIdIsEnableAsync(long id);
        Task<PermissionDTO[]> GetByTypeIdAsync(long id);
    }
}
