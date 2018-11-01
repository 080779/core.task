using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IIdNameService : IServiceSupport
    {
        Task<long> AddAsync(string name, string description, int sort, long typeId);
        Task<long> EditAsync(long id, string name, string description, int sort);
        Task<bool> FrozenAsync(long id);
        Task<bool> DelAsync(long id);
        Task<IdNameDTO> GetModelByIdAsync(long id);
        Task<IdNameDTO[]> GetByTypeIdIsEnableAsync(long id);
        Task<IdNameDTO[]> GetByTypeIdAsync(long id);
    }
}
