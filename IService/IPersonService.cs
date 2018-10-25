using DTO;
using System;
using System.Threading.Tasks;

namespace IService
{
    public interface IPersonService : IServiceSupport
    {
        long GetIdByName(string name);
    }
}
