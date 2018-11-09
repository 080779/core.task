using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LinkDTO : BaseDTO
    {
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public int IsEnabled { get; set; }
    }
}
