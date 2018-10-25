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
    /// 管理员操作日志管理类
    /// </summary>
    public class AdminLogEntity:BaseEntity
    {
        public string IpAddress { get; set; }
        public string Description { get; set; }
        public string Tip { get; set; }
        public long PermissionTypeId { get; set; }
        public long AdminId { get; set; }
        public PermissionTypeEntity PermissionType { get; set; }
        public AdminEntity Admin { get; set; }
    }
}
