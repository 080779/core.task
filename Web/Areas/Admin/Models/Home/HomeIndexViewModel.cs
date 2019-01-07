using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Models.Home
{
    public class HomeIndexViewModel
    {
        public string Name { get; set; }
        public PermissionDTO[] List { get; set; }
    }
}
