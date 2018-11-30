using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Home
{
    //[AllowAnonymous]
    public class IndexModel : PageModel
    {
        private int pageSize = 10;
        private ITaskService taskService;
        public IndexModel(ITaskService taskService)
        {
            this.taskService = taskService;
        }
        public async Task<IActionResult> OnPostGetAsync(int? within, int pageIndex = 1)
        {
            if (within != null)
            {
                var res = await taskService.GetModelListAsync(2, 7, pageIndex, pageSize);
                return new JsonResult(new AjaxResult { Status = 1, Data = res });
            }
            else
            {
                var res = await taskService.GetModelListAsync(2, null, pageIndex, pageSize);
                return new JsonResult(new AjaxResult { Status = 1, Data = res });
            }
        }
    }
}