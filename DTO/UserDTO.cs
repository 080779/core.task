using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserDTO : BaseDTO
    {
        public string UserCode { get; set; }
        public long RecommendId { get; set; }
        public string RecommendCode { get; set; }
        public string RecommendPath { get; set; }
        public int RecommendGenera { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string HeadPic { get; set; }
        public decimal Amount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal FrozenAmount { get; set; }
        public decimal RegAmount { get; set; }
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public string AccountHolder { get; set; }//银行账户持有人
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public int Enabled { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class MemberTreeDTO
    {
        public long Id { get; set; }
        public string Mobile { get; set; }
        public decimal Amount { get; set; }
        public string LevelName { get; set; }
        public long Count { get; set; }
    }
}
