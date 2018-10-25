using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    public class PersonEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int IsDeleted { get; set; } = 0;
    }
}
