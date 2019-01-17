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
    public class SlideController : ControllerBase
    {
        private ILinkService linkService;
        public SlideController(ILinkService linkService)
        {
            this.linkService = linkService;
        }

        //[HttpPost]
        public async Task<AjaxResult> List()
        {
            var res = await linkService.GetModelListEnableAsync(null);
            return new AjaxResult { Status = 1, Data = res.Select(a=>new { Id=a.Id,ImgUrl=a.ImgUrl,Name=a.Name} ) };
        }
    }
}