using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class AjaxResult
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
    }
}
