using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// 下单实体类
    /// </summary>
    public class PlaceOrderEntity : BaseEntity
    {
        public long BuyerId { get; set; }
        public long ProductId { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
    }
}
