using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PermissionTypeDTO:BaseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public int IsEnabled { get; set; }
    }
}
