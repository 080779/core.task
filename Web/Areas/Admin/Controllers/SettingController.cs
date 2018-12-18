using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [PermController("参数设置")]
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
        [PermAction("修改参数")]
        public async Task<IActionResult> Edit(long id, string parm)
        {
            bool flag = await settingService.EditAsync(id, parm);
            if (!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg = "更新失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "更新成功" });
        }
        [PermAction("修改全部")]
        public async Task<IActionResult> EditAll(long id, string parm)
        {
            bool flag = await settingService.EditAsync(id, parm);
            if (!flag)
            {
                return Json(new AjaxResult { Status = 0, Msg = "更新失败" });
            }
            return Json(new AjaxResult { Status = 1, Msg = "更新成功" });
        }
    }
}