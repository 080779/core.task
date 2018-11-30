using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    //[AllowAnonymous]
    public class homeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return Redirect("/home/index");
            //return Content("test commnet");
        }
    }
}