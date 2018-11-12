using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface ILinkTypeService : IServiceSupport
    {
        Task<long> AddAsync(string name, string description);
        Task<long> EditAsync(long id, string name, string description);
        Task<bool> FrozenAsync(long id);
        Task<bool> DelAsync(long id);
        Task<LinkTypeDTO> GetModelAsync(long id);
        Task<LinkTypeDTO[]> GetModelListIsEnableAsync();
        Task<LinkTypeDTO[]> GetModelListAsync();
    }
}
