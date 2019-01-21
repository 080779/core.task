using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ProductDTO : BaseDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Inventory { get; set; }
        public string Description { get; set; }
        public int SaleNumber { get; set; }
        public bool Putaway { get; set; }
        public bool HotSale { get; set; }
        public int Enabled { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
