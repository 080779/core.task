using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Filters
{
    public class WebAuthorizeFilter : AuthorizeFilter
    {
        //public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        //{
        //    var ere = context.HttpContext.User.;
        //    var sd = context.Filters.Any(item => item is IAllowAnonymous);
        //    if (sd)
        //    {
        //        return;
        //    }
        //    var res = new ContentResult();
        //    res.Content = "AppKey已经被封禁";
        //    res.StatusCode = 401;
        //    context.Result = res;
        //    return;
        //}
        public async override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            StringValues values;
            var res = new ContentResult();
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }
            //if(context.HttpContext.Request.IsAjax())
            //{
            //    context.Result = new JsonResult(new AjaxResult { Status = 0, Data = "/admin/login/login" });
            //}
            if (!context.HttpContext.Request.Headers.TryGetValue("token", out values))
            {
                res.Content = "token不能为空";
                res.StatusCode = 401;
                context.Result = res;
                return;
            }
            res.Content = "AppKey已经被封禁";
            res.StatusCode = 401;
            context.Result = res;
            return;
        }
    }
}
