using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IService;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models.Home;

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

        public async Task<IActionResult> GetPersonId()
        {
            //long id = await adminService.AddAsync("admin","15615615616","系统管理员","1");
            var admin = await adminService.GetModelAsync(2);
            string trueName = admin.TrueName;
            return View(admin.Id);
        }
    }
}