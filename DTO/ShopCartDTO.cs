using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ShopCartDTO : BaseDTO
    {
        public long BuyerId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public int Number { get; set; }
        public int Selected { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
