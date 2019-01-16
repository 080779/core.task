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
    public class ProductService : IProductService
    {
        public ProductDTO ToDTO(ProductEntity entity)
        {
            ProductDTO dto = new ProductDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Name = entity.Name;
            dto.Enabled = entity.Enabled;
            dto.Id = entity.Id;
            dto.Number = entity.Number;
            dto.Price = entity.Price;
            return dto;
        }

        public async Task<ProductSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ProductSearchResult result = new ProductSearchResult();
                var entities = dbc.GetAll<ProductEntity>().AsNoTracking();
                if (!string.IsNullOrEmpty(keyword))
                {
                    entities = entities.Where(a => a.Name.Contains(keyword));
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
                var res = await entities.OrderBy(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.List = res.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }
    }
}
