using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    public class ProductImageEntity : BaseEntity
    {
        public string ImgSrc { get; set; }
        public ProductEntity Product { get; set; }
        public long ProductId { get; set; }
    }
}
