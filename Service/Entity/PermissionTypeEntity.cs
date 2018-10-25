using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// 权限类型实体
    /// </summary>
    public class PermissionTypeEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public int IsEnabled { get; set; } = 1;
    }
}
