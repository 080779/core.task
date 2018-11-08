﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    public class SettingEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Parm { get; set; }
        public string Description { get; set; }
        public long? LevelId { get; set; }
        public long TypeId { get; set; }
        public IdNameEntity Type { get; set; }
    }
}
