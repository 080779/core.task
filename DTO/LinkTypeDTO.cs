using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LinkTypeDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int IsEnabled { get; set; }
    }
}
