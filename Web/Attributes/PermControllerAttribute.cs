using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Attributes
{
    public class PermControllerAttribute:Attribute
    {
        public string Name { get; set; }
        public PermControllerAttribute(string name)
        {
            this.Name = name;
        }
    }
}
