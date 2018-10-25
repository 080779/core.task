using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Entity
{
    public class AdminPermissionEntity:BaseEntity
    {
        public long AdminId { get; set; }
        public AdminEntity Admin { get; set; }
        public long PermissionId { get; set; }
        public PermissionEntity Permission { get; set; }
    }
}
