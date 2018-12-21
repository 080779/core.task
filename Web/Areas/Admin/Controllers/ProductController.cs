using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [PermController("产品管理")]
    public class ProductController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
        public IActionResult List1()
        {
            return View();
        }
        [PermAction("添加产品")]
        public IActionResult Add()
        {
            return View();
        }
        [PermAction("修改产品")]
        public IActionResult Edit()
        {
            return View();
        }
        [PermAction("删除产品")]
        public IActionResult Del()
        {
            return View();
        }
    }
}