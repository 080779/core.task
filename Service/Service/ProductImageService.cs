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
    public class ProductImageService : IProductImageService
    {
        public ProductImageDTO ToDTO(ProductImageEntity entity)
        {
            ProductImageDTO dto = new ProductImageDTO();
            dto.Id = entity.Id;
            dto.ImgSrc = entity.ImgSrc;
            dto.ProductId = entity.ProductId;
            return dto;
        }

        public async Task<long> AddAsync(long productId, List<string> imageList)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                await dbc.GetAll<ProductImageEntity>().Where(a => a.ProductId == productId).ForEachAsync(a => dbc.ProductImages.Remove(a));
                await dbc.SaveChangesAsync();
                foreach (var item in imageList)
                {
                    ProductImageEntity entity = new ProductImageEntity();
                    entity.ProductId = productId;
                    entity.ImgSrc = item;
                    dbc.ProductImages.Add(entity);
                    if(item==imageList.First())
                    {
                        ProductEntity product = await dbc.GetAll<ProductEntity>().SingleOrDefaultAsync(p => p.Id == productId);
                        if(product==null)
                        {
                            return -1;
                        }
                        product.FirstImage = item;
                    }
                }
                await dbc.SaveChangesAsync();
                return 1;
            }
        }
        
        public async Task<string[]> GetModelSrcsAsync(long productId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<ProductImageEntity>().Where(p => p.ProductId == productId);
                return (await entities.ToListAsync()).Select(p=>p.ImgSrc).ToArray();
            }
        }

        public async Task<ProductImageDTO[]> GetModelListAsync(long productId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<ProductImageEntity>().Where(p => p.ProductId == productId);
                return (await entities.ToListAsync()).Select(p => ToDTO(p)).ToArray();
            }
        }
    }
}
