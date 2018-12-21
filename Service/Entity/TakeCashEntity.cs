using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// 提现实体类
    /// </summary>
    public class TakeCashEntity:BaseEntity
    {
        public long UserId { get; set; }
        public UserEntity User { get; set; }
        public int StateId { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public int TypeId { get; set; }
        public string AdminCode { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
