﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IService;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Home;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPermissionTypeService permissionTypeService;
        private readonly IAdminService adminService;
        private readonly IPersonService personService;
        public HomeController(IPermissionTypeService permissionTypeService, IAdminService adminService, IPersonService personService)
        {
            this.permissionTypeService = permissionTypeService;
            this.adminService = adminService;
            this.personService = personService;
        }
        public async Task<IActionResult> Index()
        {
            //long userId = Convert.ToInt64(Session["Platform_AdminUserId"]);
            HomeIndexViewModel model = new HomeIndexViewModel();
            model.Mobile = (await adminService.GetModelAsync(2)).Mobile;
            model.PermissionTypes = await permissionTypeService.GetModelList();
            return View(model);
        }

        public async Task<IActionResult> GetPersonId()
        {
            //long id = await adminService.AddAsync("admin","15615615616","系统管理员","1");
            var admin = await adminService.GetModelAsync(2);
            string desc1 = admin.Description;
            return View(admin.Id);
        }
    }
}