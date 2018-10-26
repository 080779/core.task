using Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// 权限实体类
    /// </summary>
    public class PermissionEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long PermissionTypeId { get; set; }
        public PermissionTypeEntity PermissionType { get; set; }
        public int Sort { get; set; } = 1;
        public int IsEnabled { get; set; } = 1;
        //public virtual ICollection<AdminEntity> Admins { get; set; } = new List<AdminEntity>();
        //public ICollection<AdminPermissionEntity> AdminPermissions { get; set; } = new List<AdminPermissionEntity>();
    }
}
