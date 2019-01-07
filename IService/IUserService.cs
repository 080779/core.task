using DTO;
using System;
using System.Threading.Tasks;

namespace IService
{
    public interface IUserService:IServiceSupport
    {
        Task<long> AddAsync(string name, string password, string nickName, string avatarUrl);
        Task<long> AddAsync(string mobile, int levelTypeId, string password, string tradePassword, string recommend, string nickName, string avatarUrl);
        Task<long> DelAsync(long id);
        Task<bool> FrozenAsync(long id);
        Task<long> EditPwdAsync(long id, string password);
        Task<long> CheckLoginAsync(string mobile, string password);
        Task<UserDTO> GetModelAsync(long id);
        Task<string> GetMobileByIdAsync(long id);
        Task<long> GetIdByMobileAsync(string mobile);
        Task<UserSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize);
        Task<MemberTreeDTO> GetMemberTreeModelAsync(long id);
        Task<MemberTreeDTO[]> GetMemberTreeListAsync(long id);
    }
    public class UserSearchResult
    {
        public UserDTO[] List { get; set; }
        public long PageCount { get; set; }
    }
}
