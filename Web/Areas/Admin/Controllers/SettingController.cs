using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class SettingController : Controller
    {
        private readonly IParameterService parameterService;
        public SettingController(IParameterService parameterService)
        {
            this.parameterService = parameterService;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> List(bool flag = true)
        {
            var model = await  parameterService.GetAllIsEnableAsync();
            return Json(new AjaxResult { Status = 1, Data = model.Where(p=>p.Parameters.Length>0) });
        }
    }
}