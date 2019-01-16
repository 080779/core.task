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
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Web.Attributes;

namespace Web.Filters
{
    public class WebAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private IAdminService adminService;
        private IPermissionService permissionService;

        public WebAuthorizationFilter(IAdminService adminService, IPermissionService permissionService)
        {
            this.adminService = adminService;
            this.permissionService = permissionService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string path = context.HttpContext.Request.Path;
            string redirect = path.Split("/")[1];

            switch (redirect.ToLower())
            {
                case "admin": await Admin(context); break;
                case "api": await Api(context); break;
                default: return;
            }
        }

        private async Task Admin(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }
            string adminId = context.HttpContext.Session.GetString("Platform_Admin_Id");
            bool isAjax = context.HttpContext.Request.IsAjax();
            string displayName = context.ActionDescriptor.DisplayName;
            if (string.IsNullOrEmpty(adminId))
            {
                if (isAjax)
                {
                    context.Result = new JsonResult(new AjaxResult { Status = 302, Data = "/admin/home/login" });
                }
                else
                {
                    context.Result = new RedirectResult("/admin/home/login");
                }
                return;
            }

            //写入权限
            //await AutoCreatePermAsync();
            var result = await ValidPermAsync(adminId, isAjax, displayName);
            if(result==null)
            {
                return;
            }
            context.Result = result;
            return;
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

        #region 根据控制器和方法上的PermControllerAttribute和PermActionAttribute,需要验证权限的方法添加权限到数据库
        /// <summary>
        /// 根据控制器和方法上的PermControllerAttribute和PermActionAttribute,需要验证权限的方法添加权限到数据库
        /// </summary>
        /// <returns></returns>
        private async Task AutoCreatePermAsync()
        {
            //初始化权限表
            //await permissionService.InitializeAsync();
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
                    string remark = item1.Name;
                    if (item1 == methods.First())
                    {
                        levelId = 0;
                        url = "/admin/" + typeRemark + "/list";
                    }
                    await permissionService.EditAsync(count, name, remark, typeName, typeRemark, url, levelId);
                }
            }
        }
        #endregion

        #region 所有含有PermActionAttribute的action验证权限
        /// <summary>
        /// 所有含有PermActionAttribute的action验证权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="isAjax"></param>
        /// <returns></returns>
        private async Task<IActionResult> ValidPermAsync(string adminId, bool isAjax, string displayName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string actionName = displayName.Split('.').Last().Split('(').First().Trim(' ');
            //string actionname =HttpUtility.UrlEncode(actionName);
            var type = assembly.DefinedTypes.Where(t => t.BaseType == typeof(Controller) && displayName.Contains(t.FullName)).First();
            var method = type.GetMethods()
                .SingleOrDefault(m => (m.ReturnParameter.ParameterType == typeof(IActionResult) ||
                m.ReturnParameter.ParameterType == typeof(Task<IActionResult>))
                && m.GetCustomAttribute(typeof(PermActionAttribute)) != null
                && m.Name == actionName);
            if(method==null)
            {
                return null;
            }

            string typeRemark = type.Name.Replace("Controller", "");
            string remark = method.Name;
            var res = await adminService.CheckPermAsync(Convert.ToInt64(adminId), typeRemark, remark);
            if (res.Key)
            {
                return null;
            }
            string message = "没有" + res.Value + "这个权限";
            //string message = "no " + name + " permission";
            if (isAjax)
            {
                return new JsonResult(new AjaxResult { Status = 0, Msg = message });
            }
            else
            {
                return new RedirectResult("/admin/home/permission?msg=" + HttpUtility.UrlEncode(message));
            }
        }
        #endregion
    }
}
