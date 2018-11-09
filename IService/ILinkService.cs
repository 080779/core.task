using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface ILinkService : IServiceSupport
    {
        Task<long> AddAsync(string name, string imgUrl, string url, int sort);
        Task<long> EditAsync(long id, string name, string imgUrl, string url, int sort);
        Task<bool> FrozenAsync(long id);
        Task<bool> DelAsync(long id);
        Task<LinkDTO> GetModelAsync(long id);
        Task<LinkSearchResult> GetModelListAsync(string keyword,DateTime? startTime,DateTime? endTime,int pageIndex,int pageSize);
    }
    public class LinkSearchResult
    {
        public LinkDTO[] Links { get; set; }
        public long PageCount { get; set; }
    }
}
