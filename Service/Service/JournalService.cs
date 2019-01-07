using Common;
using DTO;
using IService;
using Microsoft.EntityFrameworkCore;
using Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class JournalService : IJournalService
    {
        public JournalDTO ToDTO(JournalEntity entity)
        {
            JournalDTO dto = new JournalDTO();
            dto.BalanceAmount = entity.BalanceAmount;
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.InAmount = entity.InAmount;
            dto.JournalTypeId = entity.JournalTypeId;
            dto.JournalTypeName = "";
            dto.OutAmount = entity.OutAmount;
            dto.Remark = entity.Remark;
            dto.RemarkEn = entity.RemarkEn;
            dto.UserId = entity.UserId;
            //dto.Name = entity.User.Name;
            dto.IsEnabled = entity.IsEnabled;
            dto.TaskId = entity.TaskId;
            dto.ForwardId = entity.ForwardId;
            return dto;
        }

        public async Task<JournalSearchResult> GetModelListAsync(long? userId, long? journalTypeId, string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                JournalSearchResult result = new JournalSearchResult();
                var entities = dbc.GetAll<JournalEntity>().AsNoTracking().Where(j=>j.IsEnabled==1);
                if (userId != null)
                {
                    entities = entities.Where(a => a.UserId == userId);
                }
                if (journalTypeId != null)
                {
                    entities = entities.Where(a => a.JournalTypeId == journalTypeId);
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    entities = entities.Where(g => g.Remark.Contains(keyword) || g.User.Mobile.Contains(keyword) || g.User.NickName.Contains(keyword));
                }
                if (startTime != null)
                {
                    entities = entities.Where(a => a.CreateTime >= startTime);
                }
                if (endTime != null)
                {
                    entities = entities.Where(a => a.CreateTime <= endTime);
                }
                result.PageCount = (int)Math.Ceiling((await entities.LongCountAsync()) * 1.0f / pageSize);
                decimal? totalInAmount = await entities.SumAsync(j => j.InAmount);
                decimal? totalOutAmount= await entities.SumAsync(j => j.OutAmount);
                result.TotalInAmount = totalInAmount == null ? 0 : totalInAmount;
                result.TotalOutAmount = totalOutAmount == null ? 0 : totalOutAmount;
                var journalResult = await entities.Include(j => j.User).OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.List = journalResult.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }
    }
}
