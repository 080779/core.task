using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NoticeDTO : BaseDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string Tip { get; set; }
        public string Creator { get; set; }
        public DateTime FailureTime { get; set; }
        public int IsEnabled { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
