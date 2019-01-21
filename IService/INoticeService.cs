using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface INoticeService : IServiceSupport
    {
        Task<long> AddAsync(string title, string content, int enabled, long adminId);
        Task<bool> EditAsync(long id, string title, string content, int enabled);
        Task<bool> DelAsync(long id);
        Task<bool> FrozenAsync(long id);
        Task<NoticeDTO> GetModelAsync(long id);
        Task<NoticeSearchResult> GetModelListAsync(string keyword,DateTime? startTime,DateTime? endTime,int pageIndex,int pageSize);
    }
    public class NoticeSearchResult
    {
        public NoticeDTO[] List { get; set; }
        public long PageCount { get; set; }
    }
}
