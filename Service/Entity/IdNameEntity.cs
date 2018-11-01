﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// IdName实体类
    /// </summary>
    public class IdNameEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; } = 1;
        public int IsEnabled { get; set; } = 1;
        public long IdNameTypeId { get; set; }
        public IdNameTypeEntity IdNameType { get; set; }
    }
}
