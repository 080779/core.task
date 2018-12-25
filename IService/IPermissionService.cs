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
        Task InitializeAsync();
        Task<long> EditAsync(long id, string name,string remark,string typeName,string typeRemark,string url,int? levelId);
        Task<string[]> GetModelTypeListIsEnableAsync();
        Task<PermissionDTO[]> GetModelListIsEnableByTypeNameAsync(string typeName);
        Task<PermissionDTO[]> GetModelUrlListIsEnableAsync();
    }
}
