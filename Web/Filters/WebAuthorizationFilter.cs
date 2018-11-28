using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Filters
{
    public class WebAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var res = new ContentResult();
            res.Content = "AppKey已经被封禁";
            res.StatusCode = 401;
            context.Result = res;
            return;
        }
    }
}
