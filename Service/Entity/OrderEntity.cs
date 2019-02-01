using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    public class OrderEntity : BaseEntity
    {
        public string Code { get; set; }
        public long BuyerId { get; set; }
        public long AddressId { get; set; }
        public decimal TotalAmount { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public int State { get; set; }
        public int PayType { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? PayTime { get; set; }
        public DateTime? SendTime { get; set; }//发货时间
        public DateTime? ReceiveTime { get; set; }//收货时间
        public string ReceiverName { get; set; }
        public string ReceiverMobile { get; set; }
        public string ReceiverAddress { get; set; }
    }
}
