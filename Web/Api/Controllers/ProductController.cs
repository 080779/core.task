using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Model.Product;

namespace Web.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]    
    [AllowAnonymous]
    public class ProductController : ControllerBase
    {
        private IProductService productService;
        private int pageSize = 10;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost]
        public async Task<AjaxResult> List(ListParamModel model)
        {
            var res = await productService.GetApiModelListAsync(null,null,model.PageIndex,pageSize);
            return new AjaxResult { Status = 1, Data = res };
        }

        [HttpPost]
        public async Task<AjaxResult> HotSales(HotSalesParamModel model)
        {
            var res = await productService.GetApiModelListAsync(1, null, model.PageIndex, pageSize);
            return new AjaxResult { Status = 1, Data = res };
        }
    }
}