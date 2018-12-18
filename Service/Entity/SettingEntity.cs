using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    public class SettingEntity:BaseEntity
    {
        public string TypeName { get; set; }
        public int? LevelId { get; set; }
        public string Name { get; set; }
        public string Param { get; set; }
        public string Remark { get; set; }
        public int? TypeId { get; set; }
        public int IsEnabled { get; set; } = 1;
    }
}
