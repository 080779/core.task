using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int IsDeleted { get; set; } = 0;
    }
}
