using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IAdminLogService:IServiceSupport
    {
        Task<long> AddAsync(long adminId,string permTypeName,string remark,string ipAddress,string tip);
        Task<AdminLogSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize);
    }
    public class AdminLogSearchResult
    {
        public AdminLogDTO[] List { get; set; }
        public long PageCount { get; set; }
    }
}
