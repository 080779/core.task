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
        public long StateId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public long TypeId { get; set; }
        public string AdminCode { get; set; }
    }
}
