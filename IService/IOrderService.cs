using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IOrderService : IServiceSupport
    {
        //Task<long> AddAsync(int typeId, string name, string imgUrl, string url, int sort);
        Task<bool> DelAsync(long id);
        Task<OrderDTO> GetModelAsync(long id);
        Task<OrderSearchResult> GetModelListAsync(string keyword,DateTime? startTime,DateTime? endTime,int pageIndex,int pageSize);
    }
    public class OrderSearchResult
    {
        public OrderDTO[] List { get; set; }
        public long PageCount { get; set; }
    }
}
