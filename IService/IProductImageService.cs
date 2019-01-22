using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IProductImageService : IServiceSupport
    {
        Task<long> AddAsync(long productId, List<string> imageList);
        Task<string[]> GetModelSrcsAsync(long productId);
        Task<ProductImageDTO[]> GetModelListAsync(long productId);
    }
}
