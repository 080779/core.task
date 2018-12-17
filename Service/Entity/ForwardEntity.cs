﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entity
{
    /// <summary>
    /// 任务转发实体类
    /// </summary>
    public class ForwardEntity : BaseEntity
    {
        public long TaskId { get; set; }
        public TaskEntity Task { get; set; }
        public long UserId { get; set; }
        public UserEntity User { get; set; }
        public long StateId { get; set; }
        public long Count { get; set; } = 0;
        public string ImgUrl { get; set; }
    }
}
