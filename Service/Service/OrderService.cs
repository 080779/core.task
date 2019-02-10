using Common;
using Common.Enums;
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
    public class OrderService : IOrderService
    {
        public OrderDTO ToDTO(OrderEntity entity)
        {
            OrderDTO dto = new OrderDTO();
            dto.CreateTime = entity.CreateTime;
            dto.AddressId = entity.AddressId;
            dto.BuyerId = entity.BuyerId;
            dto.Code = entity.Code;
            dto.DiscountAmount = entity.DiscountAmount;
            dto.Id = entity.Id;
            dto.PayTime = entity.PayTime;
            dto.PayTypeName = entity.PayType.GetEnumName<OrderPayTypeEnum>();
            dto.ReceiverAddress = entity.ReceiverAddress;
            dto.ReceiverMobile = entity.ReceiverMobile;
            dto.ReceiverName = entity.ReceiverName;
            dto.ReceiveTime = entity.ReceiveTime;
            dto.SendTime = entity.SendTime;
            dto.StateName = entity.State.GetEnumName<OrderStateEnum>();
            dto.TotalAmount = entity.TotalAmount;
            return dto;
        }

        //public async Task<long> AddAsync(int typeId, string name, string imgUrl, string url, int sort)
        //{
        //    using (MyDbContext dbc = new MyDbContext())
        //    {
        //        LinkEntity entity = new LinkEntity();
        //        entity.Name = name;
        //        entity.ImgUrl = imgUrl;
        //        entity.Url = url;
        //        entity.Sort = sort;
        //        entity.TypeId = typeId;
        //        dbc.Links.Add(entity);
        //        await dbc.SaveChangesAsync();
        //        return entity.Id;
        //    }
        //}
        
        public async Task<bool> DelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                LinkEntity entity = await dbc.GetAll<LinkEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.Deleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<OrderDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                OrderEntity entity = await dbc.GetAll<OrderEntity>().SingleOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }
        
        public async Task<OrderSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                OrderSearchResult result = new OrderSearchResult();
                var entities = dbc.GetAll<OrderEntity>().AsNoTracking();
                if (!string.IsNullOrEmpty(keyword))
                {
                    entities = entities.Where(a => a.Code.Contains(keyword));
                }
                if (startTime != null)
                {
                    entities = entities.Where(a => a.CreateTime >= startTime);
                }
                if (endTime != null)
                {
                    entities = entities.Where(a => a.CreateTime.Year <= endTime.Value.Year && a.CreateTime.Month <= endTime.Value.Month && a.CreateTime.Day <= endTime.Value.Day);
                }
                result.PageCount = (int)Math.Ceiling((await entities.LongCountAsync()) * 1.0f / pageSize);
                var res = await entities.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.List = res.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }
    }
}
