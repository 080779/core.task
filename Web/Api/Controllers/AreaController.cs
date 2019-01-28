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
    public class AreaController : ControllerBase
    {

        [HttpPost]
        public object List()
        {
            return CommonHelper.GetFileJson("/wwwroot/admin/js/areas.json");
        }
    }
}