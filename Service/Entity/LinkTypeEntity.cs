using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// 图片链接类型实体类
    /// </summary>
    public class LinkTypeEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int IsEnabled { get; set; } = 1;
    }
}
