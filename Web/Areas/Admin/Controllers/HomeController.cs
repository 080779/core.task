using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IService;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models.Home;
using BlueFox.VerifyCode;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Text;

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
            object verifyCode = TempData["verifyCode"];
            if (verifyCode!=null && code != verifyCode.ToString())
            {
                return new JsonResult(new AjaxResult { Status = 0, Msg = "验证码错误" });
            }
            return new JsonResult(new AjaxResult { Status = 1, Msg = "登录成功", Data = "/admin/home/index1" });
        }
        [AllowAnonymous]
        public IActionResult ImgCode()
        {
            CaptchaInfo res = VerifyCodeService.Create(4,100,40);
            TempData["verifyCode"] = res.Answer;
            //byte[] bytes = Encoding.UTF8.GetBytes(res.Answer);
            //HttpContext.Session.Set("verifyCode", bytes);
            return File(res.Image, res.ContentType);
        }
    }
}