using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PermissionDTO:BaseDTO
    {
        public string Name { get; set; }
        public string Remark { get; set; }
        public int? LevelId { get; set; }
        public string TypeName { get; set; }
        public string TypeRemark { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int? Sort { get; set; }
        public int IsEnabled { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}
