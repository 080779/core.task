using Common;
using Common.Enums;
using DTO;
using IService;
using Microsoft.EntityFrameworkCore;
using Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class UserService : IUserService
    {
        public UserDTO ToDTO(UserEntity entity)
        {
            UserDTO dto = new UserDTO();
            dto.Amount = entity.Amount;
            dto.Name = entity.Name;
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.IsEnabled = entity.IsEnabled;
            dto.LevelId = entity.LevelId; 
            dto.Mobile = entity.Mobile;
            dto.NickName = entity.NickName;
            dto.HeadPic = entity.HeadPic;
            dto.AccountHolder = entity.AccountHolder;
            dto.BankName = entity.BankName;
            dto.BankAccount = entity.BankAccount;
            dto.RecommendCode = entity.RecommendCode;
            dto.RecommendGenera = entity.RecommendGenera;
            dto.RecommendId = entity.RecommendId;
            dto.RecommendPath = entity.RecommendPath;
            dto.RegAmount = entity.RegAmount;
            dto.BonusAmount = entity.BonusAmount;
            dto.FrozenAmount = entity.FrozenAmount;
            dto.UserCode = entity.UserCode;
            dto.LevelName = entity.LevelId.GetEnumName<LevelEnum>();
            return dto;
        }

        public MemberTreeDTO ToMemberTreeDTO(UserEntity entity,long count)
        {
            MemberTreeDTO dto = new MemberTreeDTO();
            dto.Id = entity.Id;
            dto.Mobile = entity.Mobile;
            dto.Amount = entity.Amount;
            dto.LevelName = entity.LevelId.GetEnumName<LevelEnum>();
            dto.Count = count;
            return dto;
        }

        public async Task<long> AddAsync(string name, string password,string nickName,string avatarUrl)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                long id = await dbc.GetEntityIdAsync<UserEntity>(u => u.Name == name);
                if (id>0)
                {
                    return -1;
                }
                UserEntity user = new UserEntity();
                user.Name = name;
                user.Salt = CommonHelper.GetCaptcha(4);
                user.Password = CommonHelper.GetMD5(password + user.Salt);
                user.NickName = string.IsNullOrEmpty(nickName) ? "无昵称" : nickName;
                user.HeadPic = string.IsNullOrEmpty(avatarUrl) ? "/images/headpic.png" : avatarUrl;
                dbc.Users.Add(user);
                await dbc.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<long> AddAsync(string mobile, int levelTypeId, string password, string tradePassword, string recommend, string nickName, string avatarUrl)
        {
            string userCode = string.Empty;

            using (MyDbContext dbc = new MyDbContext())
            {
                long userId = 0;
                do
                {
                    userCode = CommonHelper.GetNumberCaptcha(6);
                    userId = await dbc.GetEntityIdAsync<UserEntity>(u => u.UserCode == userCode);
                } while (userId != 0);

                UserEntity recUser;
                if (string.IsNullOrWhiteSpace(recommend))
                {
                    recUser = await dbc.GetAll<UserEntity>().AsNoTracking().SingleOrDefaultAsync(u => u.Id == 1);
                }
                else
                {
                    recUser = await dbc.GetAll<UserEntity>().AsNoTracking().SingleOrDefaultAsync(u => u.UserCode == recommend);
                }

                if (recUser == null)
                {
                    return -1;
                }

                if ((await dbc.GetEntityIdAsync<UserEntity>(u => u.Mobile == mobile)) > 0)
                {
                    return -2;
                }

                try
                {
                    UserEntity user = new UserEntity();
                    user.UserCode = userCode;
                    user.LevelId = levelTypeId;
                    user.Mobile = mobile;
                    user.Salt = CommonHelper.GetCaptcha(4);
                    user.Password = CommonHelper.GetMD5(password + user.Salt);
                    //user.TradePassword = "";// tradePassword;// CommonHelper.GetMD5(tradePassword + user.Salt);
                    user.NickName = string.IsNullOrEmpty(nickName) ? "无昵称" : nickName;
                    user.HeadPic = string.IsNullOrEmpty(avatarUrl) ? "/images/headpic.png" : avatarUrl;

                    user.RecommendId = recUser.Id;
                    user.RecommendGenera = recUser.RecommendGenera + 1;
                    user.RecommendPath = recUser.RecommendPath;
                    user.RecommendCode = recUser.Mobile;

                    dbc.Users.Add(user);
                    await dbc.SaveChangesAsync();

                    var userModel = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(s => s.Id == user.Id);
                    user.RecommendPath = user.RecommendPath + "-" + user.Id;
                    await dbc.SaveChangesAsync();
                    return user.Id;
                }
                catch (Exception ex)
                {
                    return -3;
                }
            }
        }

        public async Task<bool> UpdateInfoAsync(long id, string nickName, string headpic)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return false;
                }
                if (nickName != null)
                {
                    entity.NickName = nickName;
                }
                if (headpic != null)
                {
                    entity.HeadPic = headpic;
                }
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<long> DelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return -1;
                }
                entity.IsDeleted = 1;
                await dbc.SaveChangesAsync();
                return 1;
            }
        }

        public async Task<bool> FrozenAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsEnabled = entity.IsEnabled == 1 ? 0 : 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<long> EditPwdAsync(long id, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return -1;
                }
                entity.Password = CommonHelper.GetMD5(password + entity.Salt);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }  

        public async Task<long> CheckLoginAsync(string name, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Name == name);
                if (entity == null)
                {
                    return -1;
                }
                if (entity.Password != CommonHelper.GetMD5(password + entity.Salt))
                {
                    return -2;
                }
                if (entity.IsEnabled == 0)
                {
                    return -3;
                }
                return entity.Id;
            }
        }

        public async Task<long> CheckUserMobileAsync(long id, string mobile)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetEntityIdAsync<UserEntity>(u => u.Id==id && u.Mobile==mobile);
            }
        }

        public async Task<long> CheckUserNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetEntityIdAsync<UserEntity>(u => u.Name == name);
            }
        }

        public bool CheckUserId(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                long res = dbc.GetEntityId<UserEntity>(u => u.Id == id);
                if (res <= 0)
                {
                    return false;
                }
                return true;
            }
        }

        public async Task<long> BindInfoAsync(long id, string mobile, string trueName, string wechatPayCode, string aliPayCode)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity user = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return -1;
                }
                if((await dbc.GetEntityIdAsync<UserEntity>(u=>u.Mobile==mobile))>0)
                {
                    return -2;
                }
                user.Mobile = mobile;
                await dbc.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<long> ResetBindInfoAsync(long id, string mobile, string trueName, string wechatPayCode, string aliPayCode)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity user = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id && u.Mobile==mobile);
                if (user == null)
                {
                    return -1;
                }
                user.Mobile = mobile;
                await dbc.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<decimal> GetAmountByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetDecimalPropertyAsync<UserEntity>(u=>u.Id==id,u=>u.Amount);
            }
        }

        public async Task<UserDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<long> GetIdByMobileAsync(string mobile)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetEntityIdAsync<UserEntity>(u=>u.Mobile==mobile);
            }
        }

        public async Task<string> GetMobileByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetStringPropertyAsync<UserEntity>(u=>u.Id==id,u=>u.Mobile);
            }
        }

        public async Task<UserSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserSearchResult result = new UserSearchResult();
                var users = dbc.GetAll<UserEntity>().AsNoTracking();

                if (!string.IsNullOrEmpty(keyword))
                {
                    users = users.Where(a => a.Mobile.Contains(keyword) || a.NickName.Contains(keyword));
                }
                if (startTime != null)
                {
                    users = users.Where(a => a.CreateTime >= startTime);
                }
                if (endTime != null)
                {
                    users = users.Where(a => a.CreateTime <= endTime);
                }
                result.PageCount = (int)Math.Ceiling((await users.LongCountAsync()) * 1.0f / pageSize);
                var userResult = await users.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.Users = userResult.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }

        public async Task<MemberTreeDTO> GetMemberTreeModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var user = await dbc.GetAll<UserEntity>().AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return null;
                }
                long count;
                string keyword;
                if (user.UserCode == "system")
                {
                    keyword = (user.Id + "-").ToString();
                    count = await dbc.GetAll<UserEntity>().AsNoTracking().Where(u => u.RecommendPath.Contains(keyword)).LongCountAsync();
                }
                else
                {
                    keyword = ("-" + user.Id + "-").ToString();
                    count = await dbc.GetAll<UserEntity>().AsNoTracking().Where(u => u.RecommendPath.Contains(keyword)).LongCountAsync();
                }
                return ToMemberTreeDTO(user, count);
            }
        }

        public async Task<MemberTreeDTO[]> GetMemberTreeListAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var res = await dbc.GetAll<UserEntity>().Where(u => u.RecommendId == id).ToListAsync();
                List<MemberTreeDTO> list = new List<MemberTreeDTO>();
                long count;
                string keyword;
                foreach (var user in res)
                {
                    if (user.UserCode == "system")
                    {
                        keyword = (user.Id + "-").ToString();
                        count = await dbc.GetAll<UserEntity>().AsNoTracking().Where(u => u.RecommendPath.Contains(keyword)).LongCountAsync();
                    }
                    else
                    {
                        keyword = ("-" + user.Id + "-").ToString();
                        count = await dbc.GetAll<UserEntity>().AsNoTracking().Where(u => u.RecommendPath.Contains(keyword)).LongCountAsync();
                    }
                    list.Add(ToMemberTreeDTO(user, count));
                }
                return list.ToArray();
            }
        }
    }
}
