using Common;
using IService;
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
    public class WebActionFilter : IAsyncActionFilter
    {
        private IAdminService adminService;
        private IPermissionService permissionService;

        public WebActionFilter(IAdminService adminService, IPermissionService permissionService)
        {
            this.adminService = adminService;
            this.permissionService = permissionService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string path = context.HttpContext.Request.Path;
            string redirect = path.Split("/")[1];

            switch (redirect.ToLower())
            {
                case "admin": await Admin(context,next); break;
                case "api": await Api(context,next); break;
            }            
        }

        private async Task Admin(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string adminId = context.HttpContext.Session.GetString("Platform_Admin_Id");
            bool isAjax = context.HttpContext.Request.IsAjax();
            if (string.IsNullOrEmpty(adminId))
            {
                if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                {
                    
                }
                if (isAjax)
                {
                    context.Result = new JsonResult(new AjaxResult { Status = 0, Data = "/admin/login/login" });
                }
                else
                {
                    context.Result = new RedirectResult("/admin/home/login");
                }
            }

            var result = await ValidPermAsync(context, adminId, isAjax);
            if (result != null)
            {
                context.Result = result;
            }
            //写入权限
            //await AutoCreatePermAsync();
        }

        private async Task Api(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            StringValues values;
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {

            }
            if (!context.HttpContext.Request.Headers.TryGetValue("token", out values))
            {
                context.Result = new JsonResult(new AjaxResult { Status = 0, Msg = "token不能为空" });
            }
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

        /// <summary>
        /// 所有含有PermActionAttribute的action验证权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        private async Task<IActionResult> ValidPermAsync(ActionExecutingContext context,string adminId, bool isAjax)
        {
            //获得所有Web.Areas.Admin.Controllers命名空间下的控制器
            var type = context.Controller.GetType();
            var methods = type.GetMethods().Where(m => (m.ReturnParameter.ParameterType == typeof(IActionResult) || m.ReturnParameter.ParameterType == typeof(Task<IActionResult>)) && m.GetCustomAttribute(typeof(PermActionAttribute)) != null);
            if(!methods.Any())
            {
                return null;
            }
            string typeRemark = type.Name.Replace("Controller", "");
            string remark = typeRemark + "." + methods.First().Name;
            string name = await adminService.GetPermNameAsync(Convert.ToInt64(adminId), remark);
            //string message = "没有" + name + "这个权限";
            string message = "no " + name + " permission";
            if (isAjax)
            {
                return new JsonResult(new AjaxResult { Status = 0, Msg = message });
            }
            else
            {
                return new RedirectResult("/admin/home/permission?msg=" + message);
            }
        }
    }
}
