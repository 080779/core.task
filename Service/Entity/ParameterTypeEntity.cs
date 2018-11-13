using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    public class ParameterTypeEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; } = 1;
        public int IsEnabled { get; set; } = 1;
    }
}
