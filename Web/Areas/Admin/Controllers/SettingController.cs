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
        private readonly ISettingService settingService;
        public SettingController(ISettingService settingService)
        {
            this.settingService = settingService;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> List(bool flag = true)
        {
            //var model = await settingService.GetAllIsEnableAsync();
            return Json(new AjaxResult { Status = 1/*, Data = model.Where(p => p.Parameters.Length > 0)*/ });
        }

        public async Task<ActionResult> Edit(long id, string parm)
        {
            bool flag = await settingService.UpdateAsync(id, parm);
            if (!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg = "更新失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "更新成功" });
        }

        public async Task<ActionResult> EditAll(long id, string parm)
        {
            bool flag = await settingService.UpdateAsync(id, parm);
            if (!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg = "更新失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "更新成功" });
        }
    }
}