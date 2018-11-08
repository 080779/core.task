using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Test
{
    public class ListModel : PageModel
    {
        public IActionResult OnPostAdd(int age)
        {
            return new JsonResult(new { Status=1,Msg="晋级赛",Data=new { Id=1,Name="aaa"} });
        }
    }
}