using DTO;
using System;
using System.Threading.Tasks;

namespace IService
{
    /// <summary>
    /// 设置管理接口
    /// </summary>
    public interface ISettingService : IServiceSupport
    {
        Task<bool> UpdateAsync(long id, string parameter);
        Task<bool> UpdateAsync(params SettingDTO[] settings);
        Task<string> GetParmByNameAsync(string name);
        Task<SettingDTO> GetModelAsync(long id);
        Task<SettingDTO> GetModelByNameAsync(string name);
        Task<SettingDTO[]> GetModelListByDescAsync(string desc);
    }
}
