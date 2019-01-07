using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// 公告实体类
    /// </summary>
    public class NoticeEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string Tip { get; set; }
        public string Creator { get; set; }
        public DateTime FailureTime { get; set; }
        public int IsEnabled { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
