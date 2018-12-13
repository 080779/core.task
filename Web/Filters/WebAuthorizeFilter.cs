using Common;
using IService;
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
        private IAdminService adminService;

        public WebAuthorizeFilter(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        public async override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string path = context.HttpContext.Request.Path;
            string redirect = path.Split("/")[1];

            switch(redirect.ToLower())
            {
                case "admin":await Admin(context);break;
                case "api": await Api(context); break;
                default:return;
            }
        }

        private async Task Admin(AuthorizationFilterContext context)
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
            //if (!context.HttpContext.Request.Headers.TryGetValue("token", out values))
            //{
            //    res.Content = "token不能为空";
            //    res.StatusCode = 401;
            //    context.Result = res;
            //    return;
            //}
        }

        private async Task Api(AuthorizationFilterContext context)
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
                res.Content = "Api的token不能为空";
                res.StatusCode = 401;
                context.Result = res;
                return;
            }
            return;
        }
    }
}
