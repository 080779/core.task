﻿using Common;
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
using System.Text;
using System.Threading.Tasks;
using Web.Attributes;

namespace Web.Filters
{
    public class WebAuthorizeFilter : AuthorizeFilter
    {
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

            //switch(redirect.ToLower())
            //{
            //    case "admin":await Admin(context);break;
            //    case "api": await Api(context); break;
            //    default:return;
            //}
            if(redirect.ToLower()=="admin")
            {
                string adminId = context.HttpContext.Session.GetString("Platform_Admin_Id");
                bool isAjax = context.HttpContext.Request.IsAjax();
                string displayName = context.ActionDescriptor.DisplayName;
                if (string.IsNullOrEmpty(adminId))
                {
                    if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                    {
                        return;
                    }
                    if (isAjax)
                    {
                        context.Result = new JsonResult(new AjaxResult { Status = 0, Data = "/admin/login/login" });
                    }
                    else
                    {
                        context.Result = new RedirectResult("/admin/home/login");
                    }
                    return;
                }
                if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                {
                    return;
                }
                Assembly assembly = Assembly.GetExecutingAssembly();
                string actionName = displayName.Split('.').Last().Split('(').First().ToString();
                var type = assembly.DefinedTypes.Where(t => t.BaseType == typeof(Controller) && displayName.Contains(t.FullName)).First();
                IEnumerable<MethodInfo> methods = type.GetMethods().Where(m => (m.ReturnParameter.ParameterType == typeof(IActionResult) || m.ReturnParameter.ParameterType == typeof(Task<IActionResult>)) && m.GetCustomAttribute(typeof(PermActionAttribute)) != null);
                var method = methods.SingleOrDefault();
                string typeRemark = type.Name.Replace("Controller", "");
                string remark = typeRemark + "." + method.Name;
                string name = await adminService.GetPermNameAsync(Convert.ToInt64(adminId), remark);
                string message = "没有" + name + "这个权限";
                if (isAjax)
                {
                    context.Result = new JsonResult(new AjaxResult { Status = 0, Msg = message });
                    return;
                }
                else
                {
                    context.Result = new RedirectResult("/admin/home/permission?msg=" + message);
                    return;
                }
            }
        }

        private async Task Admin(AuthorizationFilterContext context)
        {
            string adminId = context.HttpContext.Session.GetString("Platform_Admin_Id");
            bool isAjax = context.HttpContext.Request.IsAjax();
            string displayName = context.ActionDescriptor.DisplayName;
            if (string.IsNullOrEmpty(adminId))
            {
                if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                {
                    return;
                }
                if (isAjax)
                {
                    context.Result = new JsonResult(new AjaxResult { Status = 0, Data = "/admin/login/login" });
                }
                else
                {
                    context.Result = new RedirectResult("/admin/home/login");
                }
                return;
            }
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            string actionName = displayName.Split('.').Last().Split('(').First().ToString();
            var type = assembly.DefinedTypes.Where(t => t.BaseType == typeof(Controller) && displayName.Contains(t.FullName)).First();
            IEnumerable<MethodInfo> methods = type.GetMethods().Where(m => (m.ReturnParameter.ParameterType == typeof(IActionResult) || m.ReturnParameter.ParameterType == typeof(Task<IActionResult>)) && m.GetCustomAttribute(typeof(PermActionAttribute)) != null);
            var method = methods.SingleOrDefault();
            string typeRemark = type.Name.Replace("Controller", "");
            string remark = typeRemark + "." + method.Name;
            string name = await adminService.GetPermNameAsync(Convert.ToInt64(adminId), remark);
            string message = "没有" + name + "这个权限";
            if (isAjax)
            {
                context.Result = new JsonResult(new AjaxResult { Status = 0, Msg = message });
                return;
            }
            else
            {
                context.Result = new RedirectResult("/admin/home/permission?msg=" + message);
                return;
            }
            //写入权限
            //await AutoCreatePermAsync();
        }

        private async Task Api(AuthorizationFilterContext context)
        {
            StringValues values;
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }
            if (!context.HttpContext.Request.Headers.TryGetValue("token", out values))
            {
                context.Result = new JsonResult(new AjaxResult { Status = 0, Msg = "token不能为空" });
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

        /// <summary>
        /// 所有含有PermActionAttribute的action验证权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        private async Task<IActionResult> ValidPermAsync(string adminId, bool isAjax, string displayName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string actionName = displayName.Split('.').Last().Split('(').First().ToString();
            var type = assembly.DefinedTypes.Where(t => t.BaseType == typeof(Controller) && displayName.Contains(t.FullName)).First();
            IEnumerable<MethodInfo> methods = type.GetMethods().Where(m => (m.ReturnParameter.ParameterType == typeof(IActionResult) || m.ReturnParameter.ParameterType == typeof(Task<IActionResult>)) && m.GetCustomAttribute(typeof(PermActionAttribute)) != null);
            
            //IEnumerable<string> names = methods.Select(m => m.Name);
            //if(!names.Contains(actionName))
            //{
            //    return null;
            //}
            var method = methods.SingleOrDefault();
            string typeRemark = type.Name.Replace("Controller", "");
            string remark = typeRemark + "." + method.Name;
            string name = await adminService.GetPermNameAsync(Convert.ToInt64(adminId), remark);
            string message = "没有" + name + "这个权限";
            //string message = "no " + name + " permission";
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
