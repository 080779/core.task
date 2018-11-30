using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Filters
{
    public static class FilterExtensions
    {
        public static bool IsAjax(this HttpRequest request)
        {
            bool result = false;
            var xreq = request.Headers.ContainsKey("x-requested-with");
            if (xreq)
            {
                result = request.Headers["x-requested-with"] == "XMLHttpRequest";
            }
            return result;
        }
    }
}
