using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ProductImageDTO : BaseDTO
    {
        public string ImgSrc { get; set; }
        public long ProductId { get; set; }
    }
}
