using DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IService
{
    public interface IAdminService : IServiceSupport
    {
        Task<long> AddAsync(string name, string mobile, string trueName, string password);
        Task<bool> EditAsync(long id, List<long> permissionIds);
        Task<bool> EditAsync(long id, string password);
        Task<bool> DelAsync(long id);
        Task<bool> FrozenAsync(long id);
        Task<string> GetMobileByIdAsync(long id);
        Task<string> GetNameByIdAsync(long id);
        Task<AdminDTO> GetModelAsync(long id);
        Task<AdminSearchResult> GetModelListAsync(string isAdmin, string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize);
        Task<string> GetPermNameAsync(long adminId, string remark);
        Task<long> CheckLoginAsync(string name, string password);
        Task<bool> DelAll();
    }
    public class AdminSearchResult
    {
        public AdminDTO[] Admins { get; set; }
        public long PageCount { get; set; }
    }
}
