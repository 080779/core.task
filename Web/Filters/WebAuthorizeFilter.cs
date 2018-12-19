using Common;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Web.Attributes;

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
        private IPermissionService permissionService;

        public WebAuthorizeFilter(IAdminService adminService, IPermissionService permissionService)
        {
            this.adminService = adminService;
            this.permissionService = permissionService;
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
            //var code = context.HttpContext.Session.GetString("code");
            //写入权限
            await AutoCreatePermAsync();

            StringValues values;
            var res = new ContentResult();
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }
            if (context.HttpContext.Request.IsAjax())
            {
                context.Result = new JsonResult(new AjaxResult { Status = 0, Data = "/admin/login/login" });
            }
            else
            {
                context.Result = new RedirectResult("/admin/home/login");
            }
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

        /// <summary>
        /// 根据控制器和方法上的PermControllerAttribute和PermActionAttribute,需要验证权限的方法添加权限到数据库
        /// </summary>
        /// <returns></returns>
        private async Task AutoCreatePermAsync()
        {
            //初始化权限表
            await permissionService.InitializeAsync();
            Assembly assembly = Assembly.GetExecutingAssembly();
            long count = 0;
            //获得所有包含PermControllerAttribute的控制器
            var types = assembly.DefinedTypes.Where(t => t.BaseType == typeof(Controller) && t.Namespace == "Web.Areas.Admin.Controllers" && t.GetCustomAttribute(typeof(PermControllerAttribute)) != null);
            foreach (var item in types)
            {
                string typeRemark = item.Name.Replace("Controller", "");
                string typeName = ((PermControllerAttribute)item.GetCustomAttributes(typeof(PermControllerAttribute), false)[0]).Name;
                var methods = item.GetMethods().Where(m => (m.ReturnParameter.ParameterType == typeof(IActionResult) || m.ReturnParameter.ParameterType == typeof(Task<IActionResult>)) && m.GetCustomAttribute(typeof(PermActionAttribute)) != null && ((PermActionAttribute)m.GetCustomAttributes(typeof(PermActionAttribute), false)[0]).Name != null);
                foreach (var item1 in methods)
                {
                    count++;
                    int? levelId = null;
                    string url = null;
                    string name = ((PermActionAttribute)item1.GetCustomAttributes(typeof(PermActionAttribute), false)[0]).Name;
                    string remark = typeRemark + "." + item1.Name;
                    if (item1 == methods.First())
                    {
                        levelId = 0;
                        url = "/admin/" + typeRemark + "/list";
                    }
                    await permissionService.EditAsync(count, name, remark, typeName, typeRemark, url, levelId);
                }
            }
        }
    }
}
