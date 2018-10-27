using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class TaskController : Controller
    {
        #region 构造函数注入
        private readonly ITaskService taskService;
        private readonly int pageSize = 10;
        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }
        #endregion

        #region 权限类别列表
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> List(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex = 1)
        {
            var result = await taskService.GetModelListAsync(null, keyword, startTime, endTime, pageIndex, pageSize);
            return Json(new AjaxResult { Status = 1, Data = result });
        }
        #endregion
    }
}