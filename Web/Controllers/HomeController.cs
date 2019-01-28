using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;
        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            //return Redirect("/home/index");
            return View();
        }

        public IActionResult Add()
        {
            userService.AddAsync("15646652331", 1, "1", null, "446209", null, null);
            return Json(new AjaxResult { Status = 1, Msg = "" });
        }
    }
}