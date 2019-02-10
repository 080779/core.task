using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Model.Order
{
    public class ListParamModel
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
