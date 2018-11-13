using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    public class ParameterEntity : BaseEntity
    {
        public string Name { get; set; }
        public string StringValue { get; set; }
        public decimal DecimalValue { get; set; } = 1;
        public string Remark { get; set; }
        public long TypeId { get; set; }
        public ParameterTypeEntity Type { get; set; }
        public int Sort { get; set; } = 1;
        public int IsEnabled { get; set; } = 1;
    }
}
