using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]    
    [AllowAnonymous]
    public class NoticeController : ControllerBase
    {
        private INoticeService noticeService;
        public NoticeController(INoticeService noticeService)
        {
            this.noticeService = noticeService;
        }

        //[HttpPost]
        public async Task<AjaxResult> List()
        {
            var res = await noticeService.GetModelListAsync(null,null,null,1,20);
            return new AjaxResult { Status = 1, Data = res.List.Select(a=>new { Id=a.Id,Title=a.Title,Content=a.Content} ) };
        }
    }
}