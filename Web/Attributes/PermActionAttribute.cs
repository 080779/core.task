using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Attributes
{
    public class PermActionAttribute : Attribute
    {
        public string Name { get; set; }
        public PermActionAttribute(string name = null)
        {
            this.Name = name;
        }
    }
}
