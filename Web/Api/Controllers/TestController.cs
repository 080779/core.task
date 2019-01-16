using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]    
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        public object get()
        {
            List<Person> list = new List<Person>();
            list.Add(new Person { Id = 1, Name = "scsd", Src = "1.jpg" });
            list.Add(new Person { Id = 2, Name = "2354", Src = "2.jpg" });
            list.Add(new Person { Id = 3, Name = "sdh", Src = "3.jpg" });
            list.Add(new Person { Id = 4, Name = "3ret4", Src = "4.jpg" });
            list.Add(new Person { Id = 5, Name = "uyy", Src = "5.jpg" });
            return new AjaxResult { Status = 1, Data = list };
            //return new AjaxResult { Status = 0, Msg = "获取数据错误" };
        }
        public class Person
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Src { get; set; }
        }
    }
}