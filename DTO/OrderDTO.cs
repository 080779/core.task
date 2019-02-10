using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class OrderDTO : BaseDTO
    {
        public string Code { get; set; }
        public long BuyerId { get; set; }
        public long AddressId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string StateName { get; set; }
        public string PayTypeName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? PayTime { get; set; }
        public DateTime? SendTime { get; set; }//发货时间
        public DateTime? ReceiveTime { get; set; }//收货时间
        public string ReceiverName { get; set; }
        public string ReceiverMobile { get; set; }
        public string ReceiverAddress { get; set; }
    }
}
