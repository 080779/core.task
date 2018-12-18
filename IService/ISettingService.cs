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
        Task<bool> EditAsync(long id, string parameter);
        Task<bool> EditAsync(params SettingDTO[] settings);
        Task<string> GetParmByNameAsync(string name);
        Task<SettingDTO> GetModelAsync(long id);
        Task<SettingDTO[]> GetModelListIsEnableAsync();
    }
}
