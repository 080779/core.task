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
        public int Number { get; set; }
        public int Enabled { get; set; } = 1;
        public DateTime CreateTime { get; set; }
    }
}
