using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Enums;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]    
    [AllowAnonymous]
    public class OrderController : ControllerBase
    {
        private INoticeService noticeService;
        public OrderController(INoticeService noticeService)
        {
            this.noticeService = noticeService;
        }

        //[HttpPost]
        public AjaxResult StateTypes()
        {
            return new AjaxResult { Status=1,Data= MyEnumHelper.GetEnumList<OrderStateEnum>() };
        }
    }
}