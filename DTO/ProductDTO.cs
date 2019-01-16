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
        public int Number { get; set; }
        public int Enabled { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
