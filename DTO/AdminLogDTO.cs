using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AdminLogDTO:BaseDTO
    {
        public string IpAddress { get; set; }
        public string Remark { get; set; }
        public string Tip { get; set; }
        public string PermTypeName { get; set; }
        public long AdminId { get; set; }
        public string AdminMobile { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
