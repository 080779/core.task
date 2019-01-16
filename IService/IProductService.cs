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
        Task<ProductSearchResult> GetModelListAsync(string keyword,DateTime? startTime,DateTime? endTime,int pageIndex,int pageSize);
    }
    public class ProductSearchResult
    {
        public ProductDTO[] List { get; set; }
        public long PageCount { get; set; }
    }
}
