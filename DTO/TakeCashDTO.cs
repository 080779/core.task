using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TakeCashDTO:BaseDTO
    {
        public long UserId { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public long StateId { get; set; }
        public decimal? Amount { get; set; }
        public string Remark { get; set; }
        public string StateName { get; set; }
        public long TypeId { get; set; }
        public string TypeName { get; set; }
        public string AdminCode { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
