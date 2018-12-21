using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IService;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models.Home;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Web.Attributes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IPermissionService permissionService;
        public HomeController( IAdminService adminService, IPermissionService permissionService)
        {
            this.adminService = adminService;
            this.permissionService = permissionService;
        }
        public async Task<IActionResult> Index()
        {
            //long userId = Convert.ToInt64(Session["Platform_AdminUserId"]);
            HomeIndexViewModel model = new HomeIndexViewModel();
            model.Name = await adminService.GetNameByIdAsync(2);
            return View(model);
        }
        public async Task<IActionResult> Index1()
        {
            var res = await permissionService.GetModelUrlListIsEnableAsync();
            return View(res);
        }
        public IActionResult Home()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Permission(string msg)
        {
            return View((object)msg);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string name, string password, string code)
        {
            if(string.IsNullOrEmpty(name))
            {
                return new JsonResult(new AjaxResult { Status = 0, Msg = "账号不能为空" });
            }
            if (string.IsNullOrEmpty(password))
            {
                return new JsonResult(new AjaxResult { Status = 0, Msg = "密码不能为空" });
            }
            if (string.IsNullOrEmpty(code))
            {
                return new JsonResult(new AjaxResult { Status = 0, Msg = "验证码不能为空" });
            }
            object verifyCode = TempData["ValidCode"];
            if (verifyCode!=null && code != verifyCode.ToString())
            {
                return new JsonResult(new AjaxResult { Status = 0, Msg = "验证码错误" });
            }
            long res = await adminService.CheckLoginAsync(name,password);
            if(res<=0)
            {
                if(res==-1 || res==-2)
                {
                    return new JsonResult(new AjaxResult { Status = 0, Msg = "账号或密码错误" });
                }
                if(res==-3)
                {
                    return new JsonResult(new AjaxResult { Status = 0, Msg = "账号已经被冻结" });
                }
                return new JsonResult(new AjaxResult { Status = 0, Msg = "登录失败"});
            }
            HttpContext.Session.SetString("Platform_Admin_Id",res.ToString());
            return new JsonResult(new AjaxResult { Status = 1, Msg = "登录成功", Data = "/admin/home/index1" });
        }
        [AllowAnonymous]
        public IActionResult ImgCode()
        {
            ValidCode validCode = new ValidCode();
            string code = validCode.CreateVerifyCode();
            TempData["ValidCode"] = code;
            return File(validCode.CodeImageGetBuffer(code), "image/jpeg");
        }
    }
}