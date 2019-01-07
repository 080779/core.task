using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IJournalService:IServiceSupport
    {
        Task<JournalSearchResult> GetModelListAsync(long? userId, long? journalTypeId, string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize);
    }
    public class JournalSearchResult
    {
        public JournalDTO[] List { get; set; }
        public long PageCount { get; set; }
        public decimal? TotalInAmount { get; set; }
        public decimal? TotalOutAmount { get; set; }
    }
}
