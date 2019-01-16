using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// 图片链接实体类
    /// </summary>
    public class LinkEntity : BaseEntity
    {
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string Url { get; set; }
        public string Tip { get; set; }
        public int TypeId { get; set; }
        public int Sort { get; set; } = 1;
        public int Enabled { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
