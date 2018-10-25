﻿using Service;
using Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// 管理员实体类
    /// </summary>
    public class AdminEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public int IsEnabled { get; set; } = 1;
        //public virtual ICollection<PermissionEntity> Permissions { get; set; } = new List<PermissionEntity>();
        //public ICollection<AdminPermissionEntity> AdminPermissions { get; set; } = new List<AdminPermissionEntity>();
    }
}