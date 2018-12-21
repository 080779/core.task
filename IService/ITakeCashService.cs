using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface ITakeCashService:IServiceSupport
    {
        Task<long> AddAsync(long userId,int payTypeId,decimal amount,string remark);
        Task<long> Confirm(long id,long adminId, bool isSuccess);
        Task<TakeCashSearchResult> GetModelListAsync(long? userId,long? stateId, string keyword, DateTime? startTime,DateTime? endTime,int pageIndex,int pageSize);
    }
    public class TakeCashSearchResult
    {
        public TakeCashDTO[] TakeCashes { get; set; }
        public long PageCount { get; set; }
    }
}
