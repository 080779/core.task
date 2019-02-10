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
using Web.Api.Model.Order;

namespace Web.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]    
    [AllowAnonymous]
    public class OrderController : ControllerBase
    {
        private IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public async Task<AjaxResult> List(ListParamModel model)
        {
            var res = await orderService.GetModelListAsync(null,null,null,model.PageIndex,model.PageSize);
            return new AjaxResult { Status = 1, Data = res };
        }

        [HttpPost]
        public AjaxResult StateTypes()
        {
            return new AjaxResult { Status=1,Data= MyEnumHelper.GetEnumList<OrderStateEnum>() };
        }
    }
}