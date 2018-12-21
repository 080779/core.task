using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public int? IsDeleted { get; set; } = 0;
    }
}
