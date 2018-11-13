using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ParameterDTO : BaseDTO
    {
        public string Name { get; set; }
        public string StringValue { get; set; }
        public decimal DecimalValue { get; set; }
        public string Remark { get; set; }
        public string TypeName { get; set; }
        public int Sort { get; set; }
        public int IsEnabled { get; set; }
    }
}
