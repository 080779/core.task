using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IProductService : IServiceSupport
    {
        Task<long> AddAsync(string name, decimal price, int inventory, int saleNumber, bool putaway, bool hotSale, string description);
        Task<bool> EditAsync(long id, string name, decimal price, int inventory, int saleNumber, bool putaway, bool hotSale, string description);
        Task<bool> DelAsync(long id);
        Task<ProductDTO> GetModelAsync(long id);
        Task<ProductSearchResult> GetModelListAsync(string keyword,DateTime? startTime,DateTime? endTime,int pageIndex,int pageSize);
    }
    public class ProductSearchResult
    {
        public ProductDTO[] List { get; set; }
        public long PageCount { get; set; }
    }
}
