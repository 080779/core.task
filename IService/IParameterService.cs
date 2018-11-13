using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IParameterService : IServiceSupport
    {
        Task<long> AddAsync(string name, string stringValue, decimal decimalValue, string remark, int sort, long typeId);
        Task<long> EditAsync(long id, string name, string stringValue, decimal decimalValue, string remark, int sort);
        Task<bool> FrozenAsync(long id);
        Task<bool> DelAsync(long id);
        Task<ParameterDTO> GetModelByIdAsync(long id);
        Task<ParameterDTO[]> GetByTypeIdIsEnableAsync(long id);
        Task<ParameterDTO[]> GetByTypeIdAsync(long id);
        Task<ParameterSettingDTO[]> GetAllIsEnableAsync();
    }
}
