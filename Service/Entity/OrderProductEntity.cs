using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    public class OrderProductEntity : BaseEntity
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public string ProductImage { get; set; }
        public int Number { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
