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
        Task<bool> DelByNameAsync(string name);
        Task<PermissionTypeDTO[]> GetModelList();
    }
}
