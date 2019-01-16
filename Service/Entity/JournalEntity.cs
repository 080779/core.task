using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    public class JournalEntity : BaseEntity
    {
        public string Remark { get; set; }
        public string RemarkEn { get; set; }
        public decimal? InAmount { get; set; }
        public decimal? OutAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public long ForwardId { get; set; } = 0;
        public long UserId { get; set; }
        public long TaskId { get; set; } = 0;
        public UserEntity User { get; set; }
        public int JournalTypeId { get; set; }
        public int Journal01 { get; set; } = 0;
        public int Enabled { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
