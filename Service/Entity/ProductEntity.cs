using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Inventory { get; set; }
        public string Description { get; set; }
        public int SaleNumber { get; set; }
        public int Putaway { get; set; }
        public int HotSale { get; set; }
        public string FirstImage { get; set; }
        public int Enabled { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
