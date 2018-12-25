using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Models.Admin
{
    public class ListViewModel
    {
        public AdminDTO[] List { get; set; }
        public List<PermTypeModel> PermTypes { get; set; }
        public long PageCount { get; set; }
    }

    public class PermTypeModel
    {
        public string TypeName { get; set; }
        public PermissionDTO[] Permissions { get; set; }
    }
}
