﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SettingDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public int? LevelId { get; set; }
        public string Name { get; set; }
        public string Param { get; set; }
        public string Remark { get; set; }
        public int? TypeId { get; set; }
    }
}
