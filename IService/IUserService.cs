using DTO;
using System;
using System.Threading.Tasks;

namespace IService
{
    public interface IUserService:IServiceSupport
    {
        Task<long> AddAsync(string name, string password, string nickName, string avatarUrl);
        Task<long> AddAsync(string mobile, int levelTypeId, string password, string tradePassword, string recommend, string nickName, string avatarUrl);
        Task<bool> UpdateInfoAsync(long id,string nickName, string headpic);
        Task<long> DelAsync(long id);
        Task<bool> FrozenAsync(long id);
        Task<long> EditPwdAsync(long id, string password);
        Task<long> CheckLoginAsync(string name, string password);
        Task<long> CheckUserMobileAsync(long id, string mobile);
        Task<long> CheckUserNameAsync(string name);
        bool CheckUserId(long id);
        Task<long> BindInfoAsync(long id, string mobile,string trueName,string wechatPayCode,string aliPayCode);
        Task<long> ResetBindInfoAsync(long id, string mobile, string trueName, string wechatPayCode, string aliPayCode);
        Task<decimal> GetAmountByIdAsync(long id);
        Task<string> GetMobileByIdAsync(long id);
        Task<UserDTO> GetModelAsync(long id);
        Task<long> GetIdByMobileAsync(string mobile);
        Task<UserSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize);
        Task<MemberTreeDTO> GetMemberTreeModelAsync(long id);
        Task<MemberTreeDTO[]> GetMemberTreeListAsync(long id);
    }
    public class UserSearchResult
    {
        public UserDTO[] Users { get; set; }
        public long PageCount { get; set; }
    }
}
