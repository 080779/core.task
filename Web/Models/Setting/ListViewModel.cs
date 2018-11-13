using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Setting
{
    public class ListViewModel
    {
        public string TypeName { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public string StringValue { get; set; }
        public string Remark { get; set; }
    }
}
