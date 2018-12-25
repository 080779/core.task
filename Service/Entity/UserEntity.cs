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
    public class UserEntity : BaseEntity
    {
        public string UserCode { get; set; }
        public long RecommendId { get; set; }
        public string RecommendCode { get; set; }
        public string RecommendPath { get; set; }
        public int RecommendGenera { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal FrozenAmount { get; set; }
        public decimal RegAmount { get; set; }
        public int LevelId { get; set; }
        public string NickName { get; set; }
        public string HeadPic { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public string TradePassword { get; set; }
        public string AccountHolder { get; set; }//银行账户持有人
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public int IsEnabled { get; set; } = 1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
