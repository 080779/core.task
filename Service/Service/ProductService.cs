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
            dto.Description = entity.Description;
            dto.Enabled = entity.Enabled;
            dto.HotSale = entity.HotSale == 1 ? true : false;
            dto.Id = entity.Id;
            dto.Inventory = entity.Inventory;
            dto.Name = entity.Name;
            dto.Price = entity.Price;
            dto.Putaway = entity.Putaway == 1 ? true : false;
            dto.SaleNumber = entity.SaleNumber;
            return dto;
        }

        public async Task<long> AddAsync(string name, decimal price, int inventory, int saleNumber, bool putaway, bool hotSale, string description)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ProductEntity entity = new ProductEntity();
                entity.Name = name;
                entity.Price = price;
                entity.Inventory = inventory;
                entity.SaleNumber = saleNumber;
                entity.Putaway = putaway ? 1 : 0;
                entity.HotSale = hotSale ? 1 : 0;
                entity.Description = description;
                dbc.Products.Add(entity);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<bool> EditAsync(long id, string name, decimal price, int inventory, int saleNumber, bool putaway, bool hotSale, string description)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ProductEntity entity = await dbc.GetAll<ProductEntity>().SingleOrDefaultAsync(a => a.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.Name = name;
                entity.Price = price;
                entity.Inventory = inventory;
                entity.SaleNumber = saleNumber;
                entity.Putaway = putaway ? 1 : 0;
                entity.HotSale = hotSale ? 1 : 0;
                entity.Description = description;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ProductEntity entity = await dbc.GetAll<ProductEntity>().SingleOrDefaultAsync(a => a.Id == id);
                if(entity==null)
                {
                    return false;
                }
                entity.Deleted = 1;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<ProductDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ProductEntity entity = await dbc.GetAll<ProductEntity>().SingleOrDefaultAsync(a => a.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
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
                var res = await entities.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.List = res.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }
    }
}
