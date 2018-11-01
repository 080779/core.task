﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class IdNameDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; } = 1;
        public int IsEnabled { get; set; } = 0;
        public long TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
